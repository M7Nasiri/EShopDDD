using _01.Domain.Consts;
using _01.Domain.DomainEvents.Orders;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;

namespace _01.Domain.Entities.Aggregates.OrderAgg
{

    public sealed class Order : AggregateRoot
    {
        private readonly List<OrderItem> _items = new();


        public Guid CustomerId { get; private set; }

        public IReadOnlyCollection<OrderItem> Items =>
            _items.AsReadOnly();

        public Address ShippingAddress { get; private set; }

        public AppliedCouponSnapshot? AppliedCoupon { get; private set; }

        public DiscountSnapshot? MembershipDiscount { get; private set; }
        public Money BaseShippingCost { get; private set; } = Money.Zero;

        public Money ShippingCost =>
            (MembershipDiscount?.FreeShipping == true) ? Money.Zero : BaseShippingCost;

        public OrderStatus Status { get; private set; }

        public DomainDate CreatedAt { get; private set; }

        public Money SubTotal =>
        _items
            .Select(x => x.TotalPrice)
            .Aggregate(
                Money.Zero,
                (current, total) => current + total);


        public Money DiscountAmount
        {
            get
            {
                if (MembershipDiscount is not null)
                {
                    var discountedPrice = MembershipDiscount.Apply(SubTotal);
                    return SubTotal - discountedPrice;
                }

                if (AppliedCoupon is not null)
                {
                    var discountedPrice = SubTotal.ApplyPercentageDiscount(AppliedCoupon.Percent);
                    return SubTotal - discountedPrice;
                }

                return Money.Zero;
            }
        }

        public Money TotalPrice => (SubTotal - DiscountAmount) + ShippingCost;

        private Order()
        {
        }

        public Order(
            Guid customerId)
        {

            Id = Guid.NewGuid();
            CustomerId = customerId;
            Status = OrderStatus.Draft;
            CreatedAt = DomainDate.Now;
        }

        public void AddItem(
            Guid productId,
            Quantity quantity,
            Money unitPrice)
        {
            EnsureCanModify();

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

        public void RemoveItem(Guid productId)
        {
            EnsureCanModify();


            var item =
                _items.FirstOrDefault(
                    x => x.ProductId == productId);

            if (item is null)
                throw new EShopDomainException(
                    "Order item not found.");

            _items.Remove(item);
        }

        public void ChangeItemQuantity(
            Guid productId,
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
        public void SetShippingCost(Money cost)
        {
            EnsureCanModify();
            ArgumentNullException.ThrowIfNull(cost);

            BaseShippingCost = cost;
        }

        public void SetShippingAddress(
            Address address)
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


            MembershipDiscount = discount ?? throw new EShopDomainException(
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
        public void MarkAsRefunded()
        {
            if (Status != OrderStatus.Paid)
                throw new EShopDomainException("تنها سفارش‌های پرداخت‌شده قابل استرداد (Refund) هستند.");

            ChangeStatus(OrderStatus.Cancelled);

    
            var cancelledItems = _items
                .Select(i => new OrderCancelledItemDto(i.ProductId, i.Quantity.Value))
                .ToList();

            AddDomainEvent(
                new OrderCancelledDomainEvent(
                    Id,
                    CustomerId,
                    cancelledItems));
        }

        public void Ship()
        {
            ChangeStatus(OrderStatus.Shipped);
        }

        public void Deliver()
        {
            ChangeStatus(OrderStatus.Delivered);
        }

        //public void Cancel()
        //{
        //    if (Status == OrderStatus.Cancelled)
        //        return;

        //    ChangeStatus(OrderStatus.Cancelled);

        //    AddDomainEvent(
        //        new OrderCancelledDomainEvent(
        //            Id,
        //            CustomerId));
        //}
       

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
