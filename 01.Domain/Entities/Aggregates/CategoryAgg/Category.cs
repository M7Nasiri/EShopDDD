using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;

namespace _01.Domain.Entities.Aggregates.CategoryAgg
{

    public sealed class Category : AggregateRoot
    {

        public Name Name { get; private set; }

        public Guid? ParentCategoryId { get; private set; }

        private Category()
        {
        }


        public Category(
           Name name,
           Guid? parentCategoryId = null)
        {
            ArgumentNullException.ThrowIfNull(name);

            Id = Guid.NewGuid(); ;

            ValidateParent(Id, parentCategoryId);

            Name = name;
            ParentCategoryId = parentCategoryId;
        }

        public static Category Create(
             Name name,
             Guid? parentCategoryId = null)
        {
            return new Category(name, parentCategoryId);
        }



        public void EnsureCanBeDeleted(bool hasChildren, bool hasProducts)
        {
            if (hasChildren)
            {
                throw new EShopDomainException(
                    "Category cannot be deleted because it has child categories.");
            }

            if (hasProducts)
            {
                throw new EShopDomainException(
                    "Category cannot be deleted because products are assigned to it.");
            }
        }

        public void ChangeName(Name name)
        {
            ArgumentNullException.ThrowIfNull(name);

            Name = name;
        }

        public void ChangeParent(Guid? parentCategoryId)
        {
            if (parentCategoryId == Id)
                throw new EShopDomainException(
                    "Category cannot be its own parent.");

            ParentCategoryId = parentCategoryId;
        }
        private static void ValidateParent(
            Guid categoryId,
            Guid? parentCategoryId)
        {
            if (parentCategoryId is null)
                return;

            if (parentCategoryId == categoryId)
            {
                throw new EShopDomainException(
                    "Category cannot be its own parent.");
            }
        }
    }
}
