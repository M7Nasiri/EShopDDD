using EShop.Shared.Query.Filter;

namespace EShop.Query.ShipmentAgg.DTOs.Admin;

public class ShipmentAdminFilterParams : BaseFilterParam
{
    public string? Search { get; set; } 
    public string? Status { get; set; } 
    public string? TrackingCode { get; set; }
    public string? Province { get; set; }
    public string? City { get; set; }
    public Guid? OrderId { get; set; }
}