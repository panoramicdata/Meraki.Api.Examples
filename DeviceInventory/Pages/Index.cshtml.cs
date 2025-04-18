using Meraki.Api;
using Meraki.Api.Data;
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
			UserAgent = "DeviceInventory/1.0",
		}, logger);

		var organizations = await merakiClient.Organizations.GetOrganizationsAsync();

		foreach (var organization in organizations)
		{
			// Get inventory devices for each organization
			var devices = await merakiClient
				.Organizations
				.InventoryDevices
				.GetOrganizationInventoryDevicesAsync(organization.Id);

			// Add devices to the list
			InventoryDevices.AddRange(devices);
		}
	}
}
