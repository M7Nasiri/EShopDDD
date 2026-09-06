using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.CartAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CartAgg.GetCartByCustomerId
{
    public record GetCartByCustomerIdQuery(Guid CustomerId) : IQuery<CartDto?>;
}
