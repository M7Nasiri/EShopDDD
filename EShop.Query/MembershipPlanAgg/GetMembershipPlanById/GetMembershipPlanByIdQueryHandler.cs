using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.MembershipPlanAgg.DTOs.Admin;
using EShop.Shared.Query;
using Microsoft.EntityFrameworkCore;

namespace EShop.Query.MembershipPlanAgg.GetMembershipPlanById;

public class GetMembershipPlanByIdQueryHandler(ShopContext dbContext)
    : IQueryHandler<GetMembershipPlanByIdQuery, MembershipPlanAdminDto?>
{
    public async Task<MembershipPlanAdminDto?> Handle(
        GetMembershipPlanByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await dbContext.Plans
            .AsNoTracking()
            .Where(p => p.Id == request.Id)
            .Select(p => new MembershipPlanAdminDto
            {
                Id = p.Id,
                Name = p.Name.Value,
                Price = p.Price.Amount,
                DiscountPercent = p.DiscountPercent,
                FreeShipping = p.FreeShipping,
                DurationInDays = p.DurationInDays.Days,
                IsActive = p.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}