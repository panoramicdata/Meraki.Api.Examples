using Meraki.Api;
using Meraki.Api.Data;
using Meraki.Api.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeviceInventory.Pages;

public class IndexModel(ILogger<IndexModel> logger) : PageModel
{
	public List<InventoryDevice> InventoryDevices { get; set; } = [];

	public async Task OnGetAsync()
	{
		using var merakiClient = new MerakiClient(new MerakiClientOptions
		{
			ApiKey = Environment.GetEnvironmentVariable("MERAKI_API_KEY") ?? string.Empty,
			UserAgent = "DeviceInventory/1.0 ExampleCompany",
			MaxAttemptCount = 50,
			ReadOnly = true,
		}, logger);

		var organizations = await merakiClient.Organizations.GetOrganizationsAsync();

		foreach (var organization in organizations)
		{
			var inventoryDevices = await merakiClient
				.Organizations
				.InventoryDevices
				.GetOrganizationInventoryDevicesAllAsync(organization.Id);

			InventoryDevices.AddRange(inventoryDevices);
		}
	}
}
