using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.ProcessOrderPaymentCallback
{
    public class ProcessOrderPaymentCallbackCommandHandler : IBaseCommandHandler<ProcessOrderPaymentCallbackCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProcessOrderPaymentCallbackCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> Handle(ProcessOrderPaymentCallbackCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetWithItemsTrackingAsync(request.OrderId, cancellationToken);
            if (order is null)
                throw new EShopDomainException("سفارش یافت نشد.");

            order.MarkAsPaid();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
    }
}
