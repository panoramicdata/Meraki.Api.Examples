using Meraki.Api;
using Meraki.Api.Data;
using Meraki.Api.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeviceInventory.Pages;

public class IndexModel(ILogger<IndexModel> logger) : PageModel
{
	public List<InventoryDevice> InventoryDevices { get; set; } = [];

	public async Task OnGetAsync(CancellationToken cancellationToken)
	{
		var apiKey = Environment.GetEnvironmentVariable("MERAKI_API_KEY") ?? throw new InvalidOperationException("API key not found in environment variables.");

		using var merakiClient = new MerakiClient(
			new MerakiClientOptions
			{
				ApiKey = apiKey,
				UserAgent = "DeviceInventory/1.0 ExampleCompany",
				ReadOnly = true,
				MaxAttemptCount = 50
			},
			logger
		);

		var organizations = await merakiClient
			.Organizations
			.GetOrganizationsAsync(cancellationToken: cancellationToken);

		foreach (var organization in organizations)
		{
			var inventoryDevices = await merakiClient
				.Organizations
				.InventoryDevices
				.GetOrganizationInventoryDevicesAllAsync(organization.Id, cancellationToken: cancellationToken);

			InventoryDevices.AddRange(inventoryDevices);
		}
	}
}
