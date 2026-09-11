using System;
using System.Collections.Generic;
using System.Text;
using _01.Domain.Consts;
using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.OrderAgg.DTOs.GetActiveDraftOrder;
using EShop.Shared.Application;
using EShop.Shared.Query;
using Microsoft.EntityFrameworkCore;

namespace EShop.Query.OrderAgg.GetActiveDraftOrder
{
    public class GetActiveDraftOrderQueryHandler(ShopContext context)
        : IQueryHandler<GetActiveDraftOrderQuery, DraftOrderDto?>
    {
        public async Task<DraftOrderDto?> Handle(
            GetActiveDraftOrderQuery request,
            CancellationToken cancellationToken)
        {
            var draftOrder = await context.Orders
                .AsNoTracking()
                .Where(o => o.CustomerId == request.CustomerId && o.Status == OrderStatus.Draft)
                .Select(o => new DraftOrderDto(
                    o.Id,
                    o.Items.Select(i => new DraftOrderItemDto(
                        i.ProductId,
                        i.Quantity.Value,
                        i.UnitPrice.Amount,
                        i.UnitPrice.Amount * i.Quantity.Value
                    )).ToList(),
                    o.BaseShippingCost.Amount,
                    o.AppliedCoupon != null ? o.AppliedCoupon.Code : null,
                    o.AppliedCoupon != null ? o.AppliedCoupon.Percent : null,
                    o.MembershipDiscount != null ? o.MembershipDiscount.Reason : null,
                    o.MembershipDiscount != null ? o.MembershipDiscount.Percent : null,
                    o.MembershipDiscount != null ? (bool?)o.MembershipDiscount.FreeShipping : null
                ))
                .FirstOrDefaultAsync(cancellationToken);

            return draftOrder;
        }
    }
}
