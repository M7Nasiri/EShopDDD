using _01.Domain.Entities.Aggregates.CouponAgg.Repository;
using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.ApplyCouponToOrder
{
    public class ApplyCouponToOrderCommandHandler : IBaseCommandHandler<ApplyCouponToOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly ICouponRepository _couponRepository;

        public ApplyCouponToOrderCommandHandler(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            ICouponRepository couponRepository)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _couponRepository = couponRepository;
        }
        public async Task<OperationResult> Handle(ApplyCouponToOrderCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = new Guid(_currentUser.UserId.Value);

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(customerId, cancellationToken);

            if (order is null)
                throw new EShopNullException("Order was not found");

            var coupon = await _couponRepository.GetByCodeAsync(request.CouponCode, cancellationToken);

            if (coupon is null || !coupon.IsActive)
                throw new EShopNullException("Coupon was not found or is inactive.");

            var appliedCoupon = new AppliedCouponSnapshot(coupon.Code, coupon.Percent);
            order.ApplyCoupon(appliedCoupon);

            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Success();




        }
    }
}
