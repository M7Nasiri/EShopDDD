using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.CartAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CartAgg.GetCartByGuestId
{
    public record GetCartByGuestIdQuery(string GuestId) : IQuery<CartDto?>;
}
