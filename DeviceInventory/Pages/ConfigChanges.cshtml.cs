using Meraki.Api;
using Meraki.Api.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeviceInventory.Pages;

public class ConfigChangeModel(ILogger<ConfigChangeModel> logger) : PageModel
{
	public List<string> ChangeLogAdminNames { get; set; } = [];

	public List<int> ChangeLogCounts { get; set; } = [];

	public async Task OnGetAsync()
	{
		using var merakiClient = new MerakiClient(new MerakiClientOptions
		{
			ApiKey = Environment.GetEnvironmentVariable("MERAKI_API_KEY") ?? string.Empty,
			UserAgent = "DeviceInventory/1.0",
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
			.GroupBy(x => x.AdminName)
			.Select(g => new { AdminName = g.Key, Count = g.Count() })
			.OrderByDescending(g => g.Count)
			.ToList();

		ChangeLogAdminNames = [.. changeLogAggregations.Select(g => g.AdminName)];
		ChangeLogCounts = [.. changeLogAggregations.Select(g => g.Count)];
	}
}
