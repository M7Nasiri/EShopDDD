using _01.Domain.Consts;
using _01.Domain.DoamainEvents.Payment;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;

namespace _01.Domain.Entities.Aggregates.PaymentAgg
{
    public sealed class Payment : AggregateRoot
    {
        public Guid Id { get; private set; }

        public Guid OrderId { get; private set; }

        public Money Amount { get; private set; }

        public PaymentMethod Method { get; private set; }

        public PaymentStatus Status { get; private set; }

        public string? GatewayTransactionId { get; private set; }
        public string? RefundTransactionId { get; private set; }

        public string? Authority { get; private set; }
        public DomainDate CreatedAt { get; private set; }

        private Payment()
        {
        }

        public Payment(
            Guid orderId,
            Money amount,
            PaymentMethod method)
        {
            ArgumentNullException.ThrowIfNull(orderId);
            ArgumentNullException.ThrowIfNull(amount);

            if (amount.Amount <= 0)
                throw new EShopDomainException(
                    "Payment amount must be greater than zero.");

            Id = Guid.New();
            OrderId = orderId;
            Amount = amount;
            Method = method;
            Status = PaymentStatus.Pending;
            CreatedAt = DomainDate.Now;
        }

        public void StartProcessing()
        {
            if (Status != PaymentStatus.Pending)
                throw new EShopDomainException(
                    "Payment cannot be processed.");

            Status = PaymentStatus.Processing;
        }

        public void MarkAsSucceeded(
            string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                throw new EShopDomainException(
                    "Transaction id is required.");

            if (Status == PaymentStatus.Succeeded)
                return;

            if (Status != PaymentStatus.Processing)
                throw new EShopDomainException(
                    "Payment is not processing.");

            GatewayTransactionId =
                transactionId.Trim();

            Status = PaymentStatus.Succeeded;

            AddDomainEvent(
                new PaymentSuccededDomainEvent(
                    Id,
                    OrderId,
                    Amount));
        }

        public void MarkAsFailed()
        {
            if (Status == PaymentStatus.Succeeded)
                throw new EShopDomainException(
                    "Succeeded payment cannot fail.");

            Status = PaymentStatus.Failed;
        }

        public void Refund()
        {
            if (Status != PaymentStatus.Succeeded)
                throw new EShopDomainException(
                    "Only successful payment can be refunded.");

            Status = PaymentStatus.Refunded;
        }
        public void MarkAsRefunded(
    string refundTransactionId)
        {
            if (string.IsNullOrWhiteSpace(refundTransactionId))
                throw new EShopDomainException(
                    "Refund transaction id is required.");

            if (Status == PaymentStatus.Refunded)
                return;

            if (Status != PaymentStatus.Succeeded)
                throw new EShopDomainException(
                    "Only successful payment can be refunded.");

            RefundTransactionId =
                refundTransactionId.Trim();

            Status = PaymentStatus.Refunded;

            AddDomainEvent(
                new PaymentRefundedDomainEvent(
                    Id,
                    OrderId,
                    Amount,
                    RefundTransactionId));
        }
        public void SetAuthority(string? authority)
        {
            if (Status != PaymentStatus.Pending && Status != PaymentStatus.Processing)
                throw new EShopDomainException("امکان ثبت یا تغییر Authority برای تراکنشی که نهایی شده است وجود ندارد.");

            if (string.IsNullOrWhiteSpace(authority))
                throw new EShopDomainException("شناسه درگاه (Authority) نمی‌تواند خالی باشد.");

            Authority = authority;
        }

    }
}
