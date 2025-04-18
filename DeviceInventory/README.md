ChatGPT Prompt to generate the table:

Given this class and InventoryDevices holding a list of them, write a bootstrap filtered table to display them in ASP.NET core:

```csharp
[DataContract]
public class InventoryDevice
{
	[ApiAccess(ApiAccess.Read)]
	[DataMember(Name = "mac")]
	public string? Mac { get; set; }

	[ApiKey]
	[ApiAccess(ApiAccess.Read)]
	[DataMember(Name = "serial")]
	public string Serial { get; set; } = string.Empty;


	[ApiAccess(ApiAccess.Read)]
	[DataMember(Name = "name")]
	public string? Name { get; set; }

	[ApiAccess(ApiAccess.Read)]
	[DataMember(Name = "model")]
	public string? Model { get; set; }

	[ApiForeignKey(typeof(Network))]
	[ApiAccess(ApiAccess.Read)]
	[DataMember(Name = "networkId")]
	public string? NetworkId { get; set; }

	[ApiAccess(ApiAccess.Read)]
	[DataMember(Name = "orderNumber")]
	public string? OrderNumber { get; set; }

	[ApiAccess(ApiAccess.Read)]
	[DataMember(Name = "claimedAt")]
	public DateTime? ClaimedAt { get; set; }

	[ApiAccess(ApiAccess.Read)]
	[DataMember(Name = "tags")]
	public List<string>? Tags { get; set; }

	[ApiAccess(ApiAccess.Read)]
	[DataMember(Name = "licenseExpirationDate")]
	public DateTime? LicenseExpirationDate { get; set; }

	[ApiAccess(ApiAccess.Read)]
	[DataMember(Name = "productType")]
	public ProductType? ProductType { get; set; }

	[ApiAccess(ApiAccess.Read)]
	[DataMember(Name = "countryCode")]
	public string? CountryCode { get; set; }

	[ApiAccess(ApiAccess.Read)]
	[DataMember(Name = "usage")]
	public string? Usage { get; set; }

	[ApiAccess(ApiAccess.Read)]
	[DataMember(Name = "details")]
	public List<DeviceDetail>? Details { get; set; }
}
```