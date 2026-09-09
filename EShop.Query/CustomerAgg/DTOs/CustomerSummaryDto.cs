using EShop.Shared.Query;

namespace EShop.Query.CustomerAgg.DTOs;

public sealed class CustomerSummaryDto : BaseDto
{
    public string FullName { get; init; } = default!;
    public string? DefaultReceiverName { get; init; }
    public string? DefaultPhoneNumber { get; init; }
    public string? DefaultProvince { get; init; }
    public string? DefaultCity { get; init; }
    public int TotalAddresses { get; init; }
}