using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.CustomerAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;
using _01.Domain.Entities.Aggregates.CustomerAgg;
using Microsoft.EntityFrameworkCore;

namespace EShop.Query.CustomerAgg.GetAddressesOfCustomer
{
    public class GetAddressesOfCustomerQueryHandler : IQueryHandler<GetAddressesOfCustomerQuery, IReadOnlyList<AddressDto>>
    {
        private readonly ShopContext _context;
        public GetAddressesOfCustomerQueryHandler(ShopContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<AddressDto>> Handle(GetAddressesOfCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _context.Set<Customer>().AsNoTracking()
                .Where(c => c.Id == request.CustoemrId).Select(c => new
                {
                    DefaultPostalCode = c.DefaultAddress != null ? c.DefaultAddress.PostalCode : null,
                    c.Addresses
                }).FirstOrDefaultAsync(cancellationToken);
            if (customer is null)
                return Array.Empty<AddressDto>();

            return customer.Addresses.Select(a => new AddressDto(
                a.Title,
                a.ReceiverName,
                a.PhoneNumber,
                a.Province,
                a.City,
                a.Street,
                a.Plaque,
                a.PostalCode,
                IsDefault: a.PostalCode == customer.DefaultPostalCode
            )).ToList();
        }
    }
}
