using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;

namespace _01.Domain.Entities.Aggregates.ProductAgg
{
    public sealed class ProductImage : BaseEntity
    {
        public ProductImage(Name imageName, int sequence)
        {
            ArgumentNullException.ThrowIfNull(imageName);

            if (sequence < 0)
                throw new EShopDomainException(
                    "Image sequence cannot be negative.");

            Id = Guid.NewGuid();
            ImageName = imageName;
            Sequence = sequence;
        }
        public Guid ProductId { get; internal set; }
        public Name ImageName { get; private set; }
        public int Sequence { get; private set; }

        internal void AssignToProduct(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new EShopDomainException(
                    "Product identifier is required.");

            ProductId = productId;
        }

        public void ChangeSequence(int sequence)
        {
            if (sequence < 0)
                throw new EShopDomainException(
                    "Image sequence cannot be negative.");

            Sequence = sequence;
        }
    }
}
