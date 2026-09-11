using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.OrderAgg.DTOs.GetAdminOrderDetails
{
    public record AdminCustomerInfoDto(
        Guid CustomerId,
        string FullName,
        string PhoneNumber,
        string Email);
}
