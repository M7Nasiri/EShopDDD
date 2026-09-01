using _01.Domain.Entities.Aggregates.CustomerAgg;
using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IBaseCommandHandler<CheckoutOrderCommand, CheckoutOrderResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;


        public CheckoutOrderCommandHandler(
            IOrderRepository orderRepository,
            ICurrentUser currentUserService,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _currentUser = currentUserService;
            _unitOfWork = unitOfWork;
        }


        public async Task<OperationResult<CheckoutOrderResult>> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {

            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = _currentUser.UserId.Value;

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(customerId, cancellationToken);
            if (order is null)
                throw new EShopDomainException("سفارش فعال یا پیش‌نویسی برای پرداخت یافت نشد.");

            order.FinalizeOrder();

            var resultData = new CheckoutOrderResult(
                order.Id,
                order.TotalPrice.Amount
            );

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<CheckoutOrderResult>.Success(resultData);
        }
    }
}
