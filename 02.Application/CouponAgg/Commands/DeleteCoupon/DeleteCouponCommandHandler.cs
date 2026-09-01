using _01.Domain.Entities.Aggregates.CategoryAgg.Repository;
using _01.Domain.Entities.Aggregates.CouponAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CouponAgg.Commands.DeleteCoupon
{
    
    public class DeleteCouponCommandHandler : IBaseCommandHandler<DeleteCouponCommand>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public DeleteCouponCommandHandler(
           ICouponRepository couponRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _couponRepository = couponRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;

        }

        public async Task<OperationResult> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
        {
            var coupon = await _couponRepository.GetTracking(request.CouponId,cancellationToken);
            if (coupon == null)
                throw new EShopNullException("Coupon was not found.");

            coupon.SoftDelete();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult.Success();
        }
    }
}
