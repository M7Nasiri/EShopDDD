using _01.Domain.Entities.Aggregates.PaymentAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.ExternalServices;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.PaymentAgg.Commands.ProcessPayment
{

    public class ProcessPaymentCallbackCommandHandler : IBaseCommandHandler<ProcessPaymentCallbackCommand,PaymentCallbackResult>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentGatewayService _paymentGatewayService;
        private readonly IUnitOfWork _unitOfWork;

        public ProcessPaymentCallbackCommandHandler(
            IPaymentRepository paymentRepository,
            IPaymentGatewayService paymentGatewayService,
             IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _paymentGatewayService = paymentGatewayService;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<PaymentCallbackResult>> Handle(ProcessPaymentCallbackCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetAsync(request.PaymentId, cancellationToken);

            if (payment is null)
                throw new EShopDomainException("تراکنش پرداخت یافت نشد.");

            if (!string.Equals(request.Status, "OK", StringComparison.OrdinalIgnoreCase))
            {
                payment.MarkAsFailed();
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var failedResult = new PaymentCallbackResult(
                    IsSuccess: false,
                    TransactionCode: null,
                    ErrorMessage: "پرداخت توسط کاربر لغو شد یا ناموفق بود.");

                return OperationResult<PaymentCallbackResult>.Success(failedResult);
            }

            // استعلام و تایید تراکنش با درگاه
            var verifyRequest = new PaymentVerificationRequest(
                Authority: request.Authority,
                Amount: payment.Amount.Amount);

            var verificationResult = await _paymentGatewayService.VerifyPaymentAsync(verifyRequest, cancellationToken);

            if (verificationResult.IsSuccess && !string.IsNullOrWhiteSpace(verificationResult.TransactionCode))
            {
                payment.MarkAsSucceeded(verificationResult.TransactionCode);
            }
            else
            {
                payment.MarkAsFailed();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = new PaymentCallbackResult(
                IsSuccess: verificationResult.IsSuccess,
                TransactionCode: verificationResult.TransactionCode,
                ErrorMessage: verificationResult.ErrorMessage);

            return OperationResult<PaymentCallbackResult>.Success(result);
        }
    }
}
