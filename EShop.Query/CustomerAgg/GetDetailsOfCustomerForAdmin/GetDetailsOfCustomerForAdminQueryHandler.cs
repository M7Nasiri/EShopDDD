using System;
using System.Collections.Generic;
using System.Text;
using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.CustomerAgg.DTOs;
using EShop.Shared.Query;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;

namespace EShop.Query.CustomerAgg.GetDetailsOfCustomerForAdmin
{
    public class GetDetailsOfCustomerForAdminQueryHandler(ShopContext context) :
        IQueryHandler<GetDetailsOfCustomerForAdminQuery, CustomerDetailsForAdminDto>
    {

        public async Task<CustomerDetailsForAdminDto> Handle(GetDetailsOfCustomerForAdminQuery request, CancellationToken cancellationToken)
        {
            return await context.Customers
                .AsNoTracking()
                .Where(c => c.Id == request.CustomerId)
                .Select(c => new CustomerDetailsForAdminDto
                {
                    Id = c.Id,
                    FullName = c.FullName.Value,
                    DefaultPostalCode = c.DefaultAddress != null ? c.DefaultAddress.PostalCode : null,
                    Addresses = c.Addresses.Select(a => new AddressDto(
                        a.Title,
                        a.ReceiverName,
                        a.PhoneNumber,
                        a.Province,
                        a.City,
                        a.Street,
                        a.Plaque,
                        a.PostalCode,
                        c.DefaultAddress != null && a.PostalCode == c.DefaultAddress.PostalCode))
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
