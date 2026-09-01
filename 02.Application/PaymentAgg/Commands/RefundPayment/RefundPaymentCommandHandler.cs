using _01.Domain.Entities.Aggregates.PaymentAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.ExternalServices;
using EShop.Shared.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.PaymentAgg.Commands.RefundPayment
{
    public class RefundPaymentCommandHandler : IBaseCommandHandler<RefundPaymentCommand>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentGatewayService _paymentGatewayService;
        private readonly IUnitOfWork _unitOfWork;

        public RefundPaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IPaymentGatewayService paymentGatewayService,
             IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _paymentGatewayService = paymentGatewayService;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetAsync(request.PaymentId, cancellationToken);

            if (payment is null)
                throw new EShopDomainException("تراکنش پرداخت یافت نشد.");

            if (string.IsNullOrWhiteSpace(payment.GatewayTransactionId))
                throw new EShopDomainException("شناسه تراکنش بانکی معتبری برای استرداد یافت نشد.");

            // فراخوانی سرویس استرداد وجه درگاه بانکی
            var refundRequest = new RefundGatewayRequest(
                TransactionCode: payment.GatewayTransactionId,
                Amount: payment.Amount.Amount,
                Description: request.Reason);

            var refundGatewayResult = await _paymentGatewayService.RefundAsync(refundRequest, cancellationToken);

            if (!refundGatewayResult.IsSuccess)
            {
                throw new EShopDomainException(
                    $"عملیات بانکی استرداد وجه ناموفق بود: {refundGatewayResult.ErrorMessage ?? "خطای ناشناخته"}");
            }

            // تغییر وضعیت مدل در دامین و ثبت Domain Event
            payment.MarkAsRefunded(refundGatewayResult.RefundTrackingNumber ?? Guid.NewGuid().ToString("N")[..8]);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult.Success();
        }
    }
}
