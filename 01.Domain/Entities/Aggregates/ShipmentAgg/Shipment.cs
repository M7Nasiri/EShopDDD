using _01.Domain.Consts;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.ShipmentAgg
{
    public sealed class Shipment : AggregateRoot
    {
        public Id Id { get; private set; }

        public Id OrderId { get; private set; }

        public ShippingAddress Address { get; private set; }

        public ShipmentStatus Status { get; private set; }

        public string? TrackingCode { get; private set; }

        private Shipment()
        {
        }

        public Shipment(
            Id id,
            Id orderId,
            ShippingAddress address)
        {
            ArgumentNullException.ThrowIfNull(id);
            ArgumentNullException.ThrowIfNull(orderId);
            ArgumentNullException.ThrowIfNull(address);

            Id = id;
            OrderId = orderId;
            Address = address;
            Status = ShipmentStatus.Preparing;
        }

        public void MarkAsShipped(
            string trackingCode)
        {
            if (Status != ShipmentStatus.Preparing)
                throw new EShopDomainException(
                    "Shipment cannot be shipped.");

            if (string.IsNullOrWhiteSpace(trackingCode))
                throw new EShopDomainException(
                    "Tracking code is required.");

            TrackingCode =
                trackingCode.Trim();

            Status = ShipmentStatus.Shipped;
        }

        public void MarkAsDelivered()
        {
            if (Status != ShipmentStatus.Shipped)
                throw new EShopDomainException(
                    "Shipment must be shipped first.");

            Status = ShipmentStatus.Delivered;
        }

        public void Return()
        {
            if (Status == ShipmentStatus.Delivered)
            {
                Status = ShipmentStatus.Returned;
                return;
            }

            if (Status != ShipmentStatus.Shipped)
                throw new EShopDomainException(
                    "Only shipped or delivered shipment can be returned.");

            Status = ShipmentStatus.Returned;
        }
    }
}
