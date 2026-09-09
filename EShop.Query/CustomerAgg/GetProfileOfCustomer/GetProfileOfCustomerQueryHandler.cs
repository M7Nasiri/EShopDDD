using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.CustomerAgg.DTOs;
using EShop.Shared.Query;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CustomerAgg.GetProfileOfCustomer
{
    public class GetProfileOfCustomerQueryHandler : IQueryHandler<GetProfileOfCustomerQuery, CustomerProfileDto>
    {
        private readonly ShopContext _context;
        public GetProfileOfCustomerQueryHandler(ShopContext context)
        {
            _context = context;
        }
        public async Task<CustomerProfileDto> Handle(GetProfileOfCustomerQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers.Where(c => c.Id == request.CustomerId)
                .Select(cu => new CustomerProfileDto
                (cu.Id,cu.FullName.Value, cu.DefaultAddress.MapToAddressDto()
                )).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
