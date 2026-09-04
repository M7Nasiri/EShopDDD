using _01.Domain.DoamainEvents.Products;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;

namespace _01.Domain.Entities.Aggregates.ProductAgg
{

    public sealed class Product : AggregateRoot
    {
        private readonly List<ProductAttribute> _attributes = new();

        public Guid Id { get; private set; }

        public Name Name { get; private set; }

        public Description Description { get; private set; }

        public Quantity Stock { get; private set; }

        public Money UnitPrice { get; private set; }

        public Guid CategoryId { get; private set; }

        public Guid CreatedByUserId { get; private set; }
        public List<ProductImage> Images { get; private set; } = new();
        public Name ImageName { get; private set; }
        public IReadOnlyCollection<ProductAttribute> Attributes =>
            _attributes.AsReadOnly();

        private Product()
        {
        }

        public Product(
            Name name,
            Description description,
            Quantity stock,
            Money unitPrice,
            Guid categoryId,
            Guid createdByUserId)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(description);
            ArgumentNullException.ThrowIfNull(stock);
            ArgumentNullException.ThrowIfNull(unitPrice);

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Stock = stock;
            UnitPrice = unitPrice;
            CategoryId = categoryId;
            CreatedByUserId = createdByUserId;
        }

        public void ChangePrice(Money newPrice)
        {
            ArgumentNullException.ThrowIfNull(newPrice);

            UnitPrice = newPrice;
        }

        public void ChangeName(Name name)
        {
            ArgumentNullException.ThrowIfNull(name);

            Name = name;
        }

        public void ChangeDescription(Description description)
        {
            ArgumentNullException.ThrowIfNull(description);

            Description = description;
        }

        public void ChangeCategory(Guid categoryId)
        {

            CategoryId = categoryId;
        }

        public void IncreaseStock(Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(quantity);

            if (quantity.Value < 1)
                throw new EShopDomainException(
                    "Increase quantity must be greater than zero.");

            Stock = new Quantity(
                Stock.Value + quantity.Value);
        }

        public void DecreaseStock(Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(quantity);

            if (quantity.Value < 1)
                throw new EShopDomainException(
                    "Decrease quantity must be greater than zero.");

            if (Stock.Value < quantity.Value)
                throw new InsufficientStockException(
                    Id,
                    Stock.Value,
                    quantity.Value);

            Stock = new Quantity(
                Stock.Value - quantity.Value);

            if (Stock.Value < 5)
            {
                AddDomainEvent(
                    new ProductStockLowDomainEvent(
                        Id,
                        Stock.Value));
            }
        }

        public bool HasEnoughStock(Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(quantity);

            return Stock.Value >= quantity.Value;
        }

        public void AddAttribute(
            string key,
            string value)
        {
            var attribute =
                new ProductAttribute(key, value);

            if (_attributes.Any(x =>
                x.Key.Equals(
                    attribute.Key,
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new EShopDomainException(
                    $"Attribute '{key}' already exists.");
            }

            _attributes.Add(attribute);
        }

        public void RemoveAttribute(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new EShopDomainException(
                    "Attribute key is required.");

            var attribute =
                _attributes.FirstOrDefault(x =>
                    x.Key.Equals(
                        key,
                        StringComparison.OrdinalIgnoreCase));

            if (attribute is null)
                throw new EShopDomainException(
                    $"Attribute '{key}' not found.");

            _attributes.Remove(attribute);
        }

        public void ChangeAttributeValue(
            string key,
            string newValue)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new EShopDomainException(
                    "Attribute key is required.");

            var oldAttribute =
                _attributes.FirstOrDefault(x =>
                    x.Key.Equals(
                        key,
                        StringComparison.OrdinalIgnoreCase));

            if (oldAttribute is null)
                throw new EShopDomainException(
                    $"Attribute '{key}' not found.");

            var newAttribute =
                new ProductAttribute(
                    oldAttribute.Key,
                    newValue);

            _attributes.Remove(oldAttribute);
            _attributes.Add(newAttribute);
        }

        public void SetProductImage(string imageName)
        {
            ArgumentNullException.ThrowIfNull(imageName);
            ImageName = new Name(imageName);
        }

        public void AddImage(ProductImage image)
        {
            image.ProductId = Id;
            Images.Add(image);
        }

        public string RemoveImage(System.Guid id)
        {
            var image = Images.FirstOrDefault(f => f.Id == id);
            if (image == null)
                throw new EShopNullException("عکس یافت نشد");

            Images.Remove(image);
            return image.ImageName.Value;
        }


    }
}
