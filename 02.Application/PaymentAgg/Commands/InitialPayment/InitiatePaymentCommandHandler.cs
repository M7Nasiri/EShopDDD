using _01.Domain.Consts;
using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.Entities.Aggregates.PaymentAgg;
using _01.Domain.Entities.Aggregates.PaymentAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.ExternalServices;
using EShop.Shared.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.PaymentAgg.Commands.InitialPayment
{
    public class InitiatePaymentCommandHandler : IBaseCommandHandler<InitiatePaymentCommand, InitiatePaymentResult>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IPaymentGatewayService _paymentGatewayService;
        private readonly IUnitOfWork _unitOfWork;

        public InitiatePaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IOrderRepository orderRepository,
            ICurrentUser currentUser,
            IPaymentGatewayService paymentGatewayService,
            IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _paymentGatewayService = paymentGatewayService;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult<InitiatePaymentResult>> Handle(InitiatePaymentCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = new Guid(_currentUser.UserId.Value);

            var orderId = request.OrderId;
            var order = await _orderRepository.GetAsync(orderId, cancellationToken);

            if (order is null)
                throw new EShopDomainException("سفارش مورد نظر یافت نشد.");

            if (order.Status != OrderStatus.PendingPayment)
                throw new EShopDomainException("سفارش در وضعیت آماده پرداخت نیست.");

            if (order.CustomerId != customerId)
                throw new EShopDomainException("این سفارش ، مربوط به شما نیست .");

            var payment = new Payment(
                order.Id,
                order.TotalPrice,
                request.Method);

            payment.StartProcessing();

            var gatewayRequest = new PaymentGatewayRequest(
             Amount: payment.Amount.Amount,
             Description: $"پرداخت سفارش شماره {payment.OrderId.Value}",
             CallbackUrl: "https://localhost:7142/api/payment/callback");

            var gatewayResponse = await _paymentGatewayService.RequestPaymentAsync(gatewayRequest, cancellationToken);

            if (!gatewayResponse.IsSuccess || string.IsNullOrWhiteSpace(gatewayResponse.RedirectUrl))
            {
                throw new EShopDomainException(gatewayResponse.ErrorMessage ?? "خطا در اتصال به درگاه پرداخت.");
            }

            payment.SetAuthority(gatewayResponse.Authority);

            await _paymentRepository.AddAsync(payment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = new InitiatePaymentResult(
           PaymentId: payment.Id.Value,
           RedirectUrl: gatewayResponse.RedirectUrl,
           Authority: gatewayResponse.Authority!);

            return OperationResult<InitiatePaymentResult>.Success(result);
        }

    }
}
