using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Shared.Application.Interfaces.ExternalServices
{
    public sealed record PaymentGatewayRequest(
     decimal Amount,
     string Description,
     string CallbackUrl,
     string? UserEmail = null,
     string? UserPhoneNumber = null);

    public sealed record PaymentGatewayResponse(
        bool IsSuccess,
        string? Authority,
        string? RedirectUrl,
        string? ErrorMessage);

    public sealed record PaymentVerificationRequest(
        string Authority,
        decimal Amount);

    public sealed record PaymentVerificationResponse(
        bool IsSuccess,
        string? TransactionCode,
        string? ErrorMessage);

    public sealed record RefundGatewayRequest(
        string TransactionCode,
        decimal Amount,
        string? Description = null);

    public sealed record RefundGatewayResponse(
        bool IsSuccess,
        string? RefundTrackingNumber,
        string? ErrorMessage);

    public interface IPaymentGatewayService
    {
        /// <summary>
        /// درخواست پرداخت و دریافت لینک انتقال به درگاه
        /// </summary>
        Task<PaymentGatewayResponse> RequestPaymentAsync(
            PaymentGatewayRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// اعتبارسنجی تراکنش پس از بازگشت کاربر از درگاه
        /// </summary>
        Task<PaymentVerificationResponse> VerifyPaymentAsync(
            PaymentVerificationRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// استرداد وجه تراکنش در صورت پشتیبانی درگاه
        /// </summary>
        Task<RefundGatewayResponse> RefundAsync(
            RefundGatewayRequest request,
            CancellationToken cancellationToken = default);
    }
}
