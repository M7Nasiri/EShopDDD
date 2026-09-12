using _01.Domain.Consts;
using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.PaymentAgg.DTOs;
using EShop.Shared.Application;
using EShop.Shared.Query;
using Microsoft.EntityFrameworkCore;

namespace EShop.Query.PaymentAgg.GetPaymentOrderStatus;

public class GetOrderPaymentStatusQueryHandler(ShopContext context)
    : IQueryHandler<GetOrderPaymentStatusQuery,OrderPaymentStatusDto?>
{
    public async Task<OrderPaymentStatusDto?> Handle(GetOrderPaymentStatusQuery request, CancellationToken cancellationToken)
    {
        var result = await context.Payments
            .AsNoTracking()
            .Where(p => p.OrderId == request.OrderId)
            .Select(p => new OrderPaymentStatusDto(
                p.OrderId,
                p.Status.ToString(),
                p.Status == PaymentStatus.Succeeded
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}