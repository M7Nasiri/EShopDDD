using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;

namespace _01.Domain.Entities.Aggregates.CustomerAgg
{

    public sealed class Customer : AggregateRoot
    {
        private readonly List<Address> _addresses = new();

        public Guid Id { get; private set; }

        public Name FullName { get; private set; }

        public IReadOnlyCollection<Address> Addresses =>
            _addresses.AsReadOnly();

        public Address? DefaultAddress { get; private set; }

        private Customer()
        {
        }

        public Customer(
            Guid id,
            Name fullName)
        {
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

            if (_addresses.Any(a => a.PostalCode == address.PostalCode))
            {
                throw new EShopDomainException("آدرسی با این عنوان یا کد پستی قبلاً ثبت شده است.");
            }
            _addresses.Add(address);

            if (DefaultAddress is null)
                DefaultAddress = address;
        }

        public void RemoveAddress(string postalCode)
        {
            var target = _addresses.FirstOrDefault(a => a.PostalCode == postalCode);
            if (target is null)
                throw new EShopDomainException("آدرس مورد نظر یافت نشد.");

            _addresses.Remove(target);

            if (DefaultAddress == target)
            {
                DefaultAddress = _addresses.FirstOrDefault();
            }
        }

        public void ChangeDefaultAddress(string postalCode)
        {
            var target = _addresses.FirstOrDefault(a => a.PostalCode == postalCode);
            if (target is null)
                throw new EShopDomainException("آدرس انتخاب شده در لیست آدرس‌های شما وجود ندارد.");

            DefaultAddress = target;
        }
        public void EditAddress(string postalCode, Address updatedAddress)
        {
            ArgumentNullException.ThrowIfNull(updatedAddress);

            var existing = _addresses.FirstOrDefault(a => a.PostalCode == postalCode);
            if (existing is null)
                throw new EShopDomainException("آدرس مورد نظر جهت ویرایش یافت نشد.");


            if (_addresses.Any(a => a.PostalCode != postalCode &&
                                   (a.PostalCode == updatedAddress.PostalCode)))
            {
                throw new EShopDomainException("آدرس دیگری با این عنوان یا کد پستی از قبل وجود دارد.");
            }

            var index = _addresses.IndexOf(existing);
            _addresses[index] = updatedAddress;

            if (DefaultAddress == existing)
            {
                DefaultAddress = updatedAddress;
            }
        }
    }
}
