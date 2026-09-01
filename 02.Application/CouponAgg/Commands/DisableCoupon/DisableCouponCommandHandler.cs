using _01.Domain.Entities.Aggregates.CouponAgg.Repository;
using _01.Domain.Exceptions;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Persistence;

namespace _02.Application.CouponAgg.Commands.DisableCoupon
{
    public class EnableCouponCommandHandler : IBaseCommandHandler<EnableCouponCommand>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EnableCouponCommandHandler(
           ICouponRepository couponRepository,
            IUnitOfWork unitOfWork)
        {
            _couponRepository = couponRepository;
            _unitOfWork = unitOfWork;

        }
        public async Task<OperationResult> Handle(EnableCouponCommand request, CancellationToken cancellationToken)
        {
            var coupon = await _couponRepository.GetTracking(request.CouponId, cancellationToken);
            if (coupon == null)
                throw new EShopNullException("Coupon was not found.");

            coupon.Disable();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult.Success();
        }
    }
}
