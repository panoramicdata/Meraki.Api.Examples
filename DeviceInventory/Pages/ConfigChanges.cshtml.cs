using Meraki.Api;
using Meraki.Api.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeviceInventory.Pages;

public class ConfigChangeModel(ILogger<ConfigChangeModel> logger) : PageModel
{
	public List<string> ChangeLogNetworkNames { get; set; } = [];

	public List<int> ChangeLogCounts { get; set; } = [];

	public async Task OnGetAsync()
	{
		var apiKey = Environment.GetEnvironmentVariable("MERAKI_API_KEY") ?? string.Empty;

		using var merakiClient = new MerakiClient(new MerakiClientOptions
		{
			ApiKey = apiKey,
			UserAgent = "DeviceInventory/1.0 ExampleCompany",
			MaxAttemptCount = 50,
			ReadOnly = true,
			MaxBackOffDelaySeconds = 60,
		}, logger);

		var organizations = await merakiClient.Organizations.GetOrganizationsAsync();

		var allChangeLogEntries = new List<ChangeLogEntry>();

		var t0 = DateTimeOffset.UtcNow.AddDays(-30).ToUnixTimeSeconds();
		var t1 = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

		foreach (var organization in organizations)
		{
			var changeLogEntries = await merakiClient
				.Organizations
				.ConfigurationChanges
				.GetOrganizationConfigurationChangesAsync(
					organization.Id,
					t0: t0.ToString(),
					t1: t1.ToString());

			allChangeLogEntries.AddRange(changeLogEntries);
		}

		var changeLogAggregations = allChangeLogEntries
			.Where(e => !string.IsNullOrEmpty(e.NetworkName))
			.GroupBy(x => x.NetworkName)
			.Select(g => new { NetworkName = g.Key, Count = g.Count() })
			.OrderByDescending(g => g.Count)
			.ToList();

		ChangeLogNetworkNames = [.. changeLogAggregations.Select(g => g.NetworkName)];
		ChangeLogCounts = [.. changeLogAggregations.Select(g => g.Count)];
	}
}
