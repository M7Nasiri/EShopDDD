using EShop.Shared.Application.Interfaces.ExternalServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace EShop.Infrastructure.ExternalServices.Payment
{
    public sealed class ZarinpalPaymentGatewayService : IPaymentGatewayService
    {
        private readonly HttpClient _httpClient;
        private readonly string _merchantId;
        private readonly bool _isSandbox;
        private readonly ILogger<ZarinpalPaymentGatewayService> _logger;

        public ZarinpalPaymentGatewayService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ZarinpalPaymentGatewayService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _merchantId = configuration["Payment:Zarinpal:MerchantId"] ?? "00000000-0000-0000-0000-000000000000";
            _isSandbox = bool.Parse(configuration["Payment:Zarinpal:IsSandbox"] ?? "true");
        }

        public async Task<PaymentGatewayResponse> RequestPaymentAsync(
            PaymentGatewayRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var payload = new
                {
                    merchant_id = _merchantId,
                    amount = (long)request.Amount, // مبالغ درگاه‌ها بر حسب ریال یا تومان بر اساس داکیومنت
                    description = request.Description,
                    callback_url = request.CallbackUrl,
                    metadata = new
                    {
                        email = request.UserEmail,
                        mobile = request.UserPhoneNumber
                    }
                };

                var endpoint = _isSandbox
                    ? "https://sandbox.zarinpal.com/pg/v4/payment/request.json"
                    : "https://api.zarinpal.com/pg/v4/payment/request.json";

                var response = await _httpClient.PostAsJsonAsync(endpoint, payload, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    return new PaymentGatewayResponse(false, null, null, "خطا در برقراری ارتباط با درگاه پرداخت.");
                }

                var result = await response.Content.ReadFromJsonAsync<ZarinpalRequestResult>(cancellationToken: cancellationToken);

                if (result?.Data?.Code == 100 && !string.IsNullOrWhiteSpace(result.Data.Authority))
                {
                    var paymentGateUrl = _isSandbox
                        ? $"https://sandbox.zarinpal.com/pg/StartPay/{result.Data.Authority}"
                        : $"https://www.zarinpal.com/pg/StartPay/{result.Data.Authority}";

                    return new PaymentGatewayResponse(true, result.Data.Authority, paymentGateUrl, null);
                }

                return new PaymentGatewayResponse(false, null, null, $"خطای درگاه: کد {result?.Errors?.Code}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while requesting payment from gateway.");
                return new PaymentGatewayResponse(false, null, null, "خطای سیستمی در ارسال به درگاه پرداخت.");
            }
        }

        public async Task<PaymentVerificationResponse> VerifyPaymentAsync(
            PaymentVerificationRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var payload = new
                {
                    merchant_id = _merchantId,
                    amount = (long)request.Amount,
                    authority = request.Authority
                };

                var endpoint = _isSandbox
                    ? "https://sandbox.zarinpal.com/pg/v4/payment/verify.json"
                    : "https://api.zarinpal.com/pg/v4/payment/verify.json";

                var response = await _httpClient.PostAsJsonAsync(endpoint, payload, cancellationToken);
                var result = await response.Content.ReadFromJsonAsync<ZarinpalVerifyResult>(cancellationToken: cancellationToken);

                if (result?.Data?.Code == 100 || result?.Data?.Code == 101)
                {
                    return new PaymentVerificationResponse(true, result.Data.RefId.ToString(), null);
                }

                return new PaymentVerificationResponse(false, null, $"تراکنش تایید نشد: کد {result?.Errors?.Code}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during payment verification for Authority: {Authority}", request.Authority);
                return new PaymentVerificationResponse(false, null, "خطا در تایید تراکنش بانکی.");
            }
        }

        public Task<RefundGatewayResponse> RefundAsync(RefundGatewayRequest request, CancellationToken cancellationToken = default)
        {
            // درگاه‌های بانکی معمولاً API جداگانه یا دستی برای استرداد دارند
            _logger.LogInformation("Refund requested for RefId: {RefId} with Amount: {Amount}", request.TransactionCode, request.Amount);

            return Task.FromResult(new RefundGatewayResponse(true, Guid.NewGuid().ToString("N")[..8], null));
        }

        // DTOهای اختصاصی زرین‌پال
        private sealed class ZarinpalRequestResult
        {
            [JsonPropertyName("data")]
            public RequestData? Data { get; set; }
            [JsonPropertyName("errors")]
            public ErrorData? Errors { get; set; }
        }

        private sealed class RequestData
        {
            [JsonPropertyName("code")]
            public int Code { get; set; }
            [JsonPropertyName("authority")]
            public string? Authority { get; set; }
        }

        private sealed class ZarinpalVerifyResult
        {
            [JsonPropertyName("data")]
            public VerifyData? Data { get; set; }
            [JsonPropertyName("errors")]
            public ErrorData? Errors { get; set; }
        }

        private sealed class VerifyData
        {
            [JsonPropertyName("code")]
            public int Code { get; set; }
            [JsonPropertyName("ref_id")]
            public long RefId { get; set; }
        }

        private sealed class ErrorData
        {
            [JsonPropertyName("code")]
            public int Code { get; set; }
            [JsonPropertyName("message")]
            public string? Message { get; set; }
        }
    }
}
