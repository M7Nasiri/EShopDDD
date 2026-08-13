using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.CategoryAgg
{

    public sealed class Category : AggregateRoot
    {
        public Id Id { get; private set; }

        public Name Name { get; private set; }

        public Id? ParentCategoryId { get; private set; }

        private Category()
        {
        }

        public Category(
            Id id,
            Name name,
            Id? parentCategoryId = null)
        {
            ArgumentNullException.ThrowIfNull(id);
            ArgumentNullException.ThrowIfNull(name);

            if (parentCategoryId == id)
                throw new EShopDomainException(
                    "Category cannot be its own parent.");

            Id = id;
            Name = name;
            ParentCategoryId = parentCategoryId;
        }

        public void ChangeName(Name name)
        {
            ArgumentNullException.ThrowIfNull(name);

            Name = name;
        }

        public void ChangeParent(Id? parentCategoryId)
        {
            if (parentCategoryId == Id)
                throw new EShopDomainException(
                    "Category cannot be its own parent.");

            ParentCategoryId = parentCategoryId;
        }
    }
}
