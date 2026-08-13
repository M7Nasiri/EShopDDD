using _01.Domain.Consts;
using _01.Domain.DoamainEvents.Orders;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.OrderAgg
{

    public sealed class Order : AggregateRoot
    {
        private readonly List<OrderItem> _items = new();

        public Id Id { get; private set; }

        public Id CustomerId { get; private set; }

        public IReadOnlyCollection<OrderItem> Items =>
            _items.AsReadOnly();

        public ShippingAddress? ShippingAddress { get; private set; }

        public AppliedCouponSnapshot? AppliedCoupon { get; private set; }

        public DiscountSnapshot? MembershipDiscount { get; private set; }

        public OrderStatus Status { get; private set; }

        public DomainDate CreatedAt { get; private set; }

        public Money SubTotal =>
            _items
                .Select(x => x.TotalPrice)
                .Aggregate(
                    Money.Zero,
                    (current, total) => current + total);

        public Money TotalPrice
        {
            get
            {
                var result = SubTotal;

                if (MembershipDiscount is not null)
                {
                    result =
                        MembershipDiscount.Apply(result);
                }

                if (AppliedCoupon is not null)
                {
                    result =
                        result.ApplyPercentageDiscount(
                            AppliedCoupon.Percent);
                }

                return result;
            }
        }

        private Order()
        {
        }

        public Order(
            Id id,
            Id customerId)
        {
            ArgumentNullException.ThrowIfNull(id);
            ArgumentNullException.ThrowIfNull(customerId);

            Id = id;
            CustomerId = customerId;
            Status = OrderStatus.Draft;
            CreatedAt = DomainDate.Now;
        }

        public void AddItem(
            Id productId,
            Quantity quantity,
            Money unitPrice)
        {
            EnsureCanModify();

            ArgumentNullException.ThrowIfNull(productId);
            ArgumentNullException.ThrowIfNull(quantity);
            ArgumentNullException.ThrowIfNull(unitPrice);

            var existingItem =
                _items.FirstOrDefault(
                    x => x.ProductId == productId);

            if (existingItem is not null)
            {
                existingItem.IncreaseQuantity(quantity);
                return;
            }

            _items.Add(
                new OrderItem(
                    productId,
                    quantity,
                    unitPrice));
        }

        public void RemoveItem(Id productId)
        {
            EnsureCanModify();

            ArgumentNullException.ThrowIfNull(productId);

            var item =
                _items.FirstOrDefault(
                    x => x.ProductId == productId);

            if (item is null)
                throw new EShopDomainException(
                    "Order item not found.");

            _items.Remove(item);
        }

        public void ChangeItemQuantity(
            Id productId,
            Quantity quantity)
        {
            EnsureCanModify();

            var item =
                _items.FirstOrDefault(
                    x => x.ProductId == productId);

            if (item is null)
                throw new EShopDomainException(
                    "Order item not found.");

            if (quantity.Value < 1)
                throw new EShopDomainException(
                    "Quantity must be greater than zero.");

            var difference =
                quantity.Value - item.Quantity.Value;

            if (difference > 0)
            {
                item.IncreaseQuantity(
                    new Quantity(difference));
            }
            else if (difference < 0)
            {
                item.DecreaseQuantity(
                    new Quantity(Math.Abs(difference)));
            }
        }

        public void SetShippingAddress(
            ShippingAddress address)
        {
            EnsureCanModify();

            ArgumentNullException.ThrowIfNull(address);

            ShippingAddress = address;
        }

        public void ApplyCoupon(
            AppliedCouponSnapshot coupon)
        {
            EnsureCanModify();

            ArgumentNullException.ThrowIfNull(coupon);

            if (AppliedCoupon is not null)
                throw new EShopDomainException(
                    "Order already has a coupon.");

            AppliedCoupon = coupon;
        }

        public void RemoveCoupon()
        {
            EnsureCanModify();

            AppliedCoupon = null;
        }

        public void ApplyMembershipDiscount(
            DiscountSnapshot discount)
        {
            EnsureCanModify();

            ArgumentNullException.ThrowIfNull(discount);

            if (MembershipDiscount is not null)
                throw new EShopDomainException(
                    "Membership discount already applied.");

            MembershipDiscount = discount;
        }

        public void RemoveMembershipDiscount()
        {
            EnsureCanModify();

            MembershipDiscount = null;
        }

        public void FinalizeOrder()
        {
            if (Status != OrderStatus.Draft)
                throw new EShopDomainException(
                    "Only draft orders can be finalized.");

            if (!_items.Any())
                throw new EShopDomainException(
                    "Order must contain at least one item.");

            if (ShippingAddress is null)
                throw new EShopDomainException(
                    "Shipping address is required.");

            if (TotalPrice.Amount <= 0)
                throw new EShopDomainException(
                    "Order total must be greater than zero.");

            Status = OrderStatus.PendingPayment;

            AddDomainEvent(
                new OrderFinalizedDomainEvent(
                    Id,
                    CustomerId,
                    TotalPrice));
        }

        public void MarkAsPaid()
        {
            ChangeStatus(OrderStatus.Paid);

            AddDomainEvent(
                new OrderPaidDomainEvent(
                    Id,
                    CustomerId,
                    TotalPrice));
        }

        public void Ship()
        {
            ChangeStatus(OrderStatus.Shipped);
        }

        public void Deliver()
        {
            ChangeStatus(OrderStatus.Delivered);
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Cancelled)
                return;

            ChangeStatus(OrderStatus.Cancelled);

            AddDomainEvent(
                new OrderCancelledDomainEvent(
                    Id,
                    CustomerId));
        }

        private void ChangeStatus(
            OrderStatus newStatus)
        {
            if (newStatus == Status)
                return;

            var allowed =
                GetAllowedTransitions(Status);

            if (!allowed.Contains(newStatus))
                throw new EShopDomainException(
                    $"Invalid order transition: " +
                    $"{Status} -> {newStatus}");

            Status = newStatus;
        }

        private static OrderStatus[] GetAllowedTransitions(
            OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Draft =>
                    [OrderStatus.PendingPayment],

                OrderStatus.PendingPayment =>
                    [
                        OrderStatus.Paid,
                    OrderStatus.Cancelled
                    ],

                OrderStatus.Paid =>
                    [
                        OrderStatus.Shipped,
                    OrderStatus.Cancelled
                    ],

                OrderStatus.Shipped =>
                    [OrderStatus.Delivered],

                OrderStatus.Delivered =>
                    [],

                OrderStatus.Cancelled =>
                    [],

                _ => []
            };
        }

        private void EnsureCanModify()
        {
            if (Status != OrderStatus.Draft)
                throw new EShopDomainException(
                    $"Order cannot be modified " +
                    $"in '{Status}' status.");
        }
    }


}
