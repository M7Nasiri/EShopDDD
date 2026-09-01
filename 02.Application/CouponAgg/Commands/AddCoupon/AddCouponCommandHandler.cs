using _01.Domain.Entities.Aggregates.CommentAgg.Repository;
using _01.Domain.Entities.Aggregates.CouponAgg;
using _01.Domain.Entities.Aggregates.CouponAgg.Repository;
using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CouponAgg.Commands.AddCoupon
{
    public class AddCouponCommandHandler : IBaseCommandHandler<AddCouponCommand>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public AddCouponCommandHandler(
           ICouponRepository couponRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _couponRepository = couponRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;

        }
        public async Task<OperationResult> Handle(AddCouponCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var userId = new Guid(_currentUser.UserId.Value);

            var isUnique = await _couponRepository.IsCodeUniqueAsync(request.Code, cancellationToken);
            if (!isUnique)
            {
                throw new EShopDomainException("کد کوپن وارد شده تکراری است.");
            }

            Coupon coupon = new Coupon(request.Code,request.Percent,userId,new DomainDate(request.StartDate),
                new DomainDate(request.EndDate),request.UsageLimit);

            await _couponRepository.AddAsync(coupon,cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Success();
        }
    }
}
