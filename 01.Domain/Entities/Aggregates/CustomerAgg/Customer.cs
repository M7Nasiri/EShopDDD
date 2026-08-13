using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Text;

namespace _01.Domain.Entities.Aggregates.CustomerAgg
{

    public sealed class Customer : AggregateRoot
    {
        private readonly List<Address> _addresses = new();

        public Id Id { get; private set; }

        public Name FullName { get; private set; }

        public IReadOnlyCollection<Address> Addresses =>
            _addresses.AsReadOnly();

        public Address? DefaultAddress { get; private set; }

        private Customer()
        {
        }

        public Customer(
            Id id,
            Name fullName)
        {
            ArgumentNullException.ThrowIfNull(id);
            ArgumentNullException.ThrowIfNull(fullName);

            Id = id;
            FullName = fullName;
        }

        public void ChangeFullName(Name name)
        {
            ArgumentNullException.ThrowIfNull(name);

            FullName = name;
        }

        public void AddAddress(Address address)
        {
            ArgumentNullException.ThrowIfNull(address);

            if (_addresses.Contains(address))
                throw new EShopDomainException(
                    "Address already exists.");

            _addresses.Add(address);

            if (DefaultAddress is null)
                DefaultAddress = address;
        }

        public void RemoveAddress(Address address)
        {
            ArgumentNullException.ThrowIfNull(address);

            if (!_addresses.Remove(address))
                throw new EShopDomainException(
                    "Address not found.");

            if (DefaultAddress == address)
            {
                DefaultAddress =
                    _addresses.FirstOrDefault();
            }
        }

        public void ChangeDefaultAddress(Address address)
        {
            ArgumentNullException.ThrowIfNull(address);

            if (!_addresses.Contains(address))
                throw new EShopAddressException(
                    "Address must belong to customer.");

            DefaultAddress = address;
        }
    }
}
