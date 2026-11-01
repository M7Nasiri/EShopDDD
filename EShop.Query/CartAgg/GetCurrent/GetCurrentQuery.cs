using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.CartAgg.DTOs;
using EShop.Shared.Query;

namespace EShop.Query.CartAgg.GetCurrent
{
    public record GetCurrentQuery : IQuery<CartDto?>;

}
