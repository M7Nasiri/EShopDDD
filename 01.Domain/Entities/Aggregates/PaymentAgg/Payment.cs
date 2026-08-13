using _01.Domain.Consts;
using _01.Domain.DoamainEvents.Payment;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.PaymentAgg
{
    public sealed class Payment : AggregateRoot
    {
        public Id Id { get; private set; }

        public Id OrderId { get; private set; }

        public Money Amount { get; private set; }

        public PaymentMethod Method { get; private set; }

        public PaymentStatus Status { get; private set; }

        public string? GatewayTransactionId { get; private set; }

        public DomainDate CreatedAt { get; private set; }

        private Payment()
        {
        }

        public Payment(
            Id id,
            Id orderId,
            Money amount,
            PaymentMethod method)
        {
            ArgumentNullException.ThrowIfNull(id);
            ArgumentNullException.ThrowIfNull(orderId);
            ArgumentNullException.ThrowIfNull(amount);

            if (amount.Amount <= 0)
                throw new EShopDomainException(
                    "Payment amount must be greater than zero.");

            Id = id;
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
                new PaymentSucceededDomainEvent(
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
    }
}
