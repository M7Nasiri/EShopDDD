using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.OrderAgg.DTOs.GetCustomerOrderDetails
{
    public record CustomerOrderItemDetailDto(
        Guid ProductId,
        string ProductTitle,
        string? ProductImageName,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice);
}
