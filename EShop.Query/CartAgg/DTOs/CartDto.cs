using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CartAgg.DTOs
{
    public class CartDto : BaseDto
    {
        public Guid? CustomerId { get; set; }
        public string? CustomerFullName { get; set; }
        public string? GuestId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();

        public decimal TotalAmount => Items.Sum(x => x.TotalPrice);
        public int TotalItemCount => Items.Sum(x => x.Quantity);
    }

}
