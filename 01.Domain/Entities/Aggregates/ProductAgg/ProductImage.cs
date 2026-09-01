using _01.Domain.Entities.Aggregates.CustomerAgg;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.ProductAgg
{
    public class ProductImage : BaseEntity
    {
        public ProductImage(Name imageName, int sequence)
        {
            ArgumentNullException.ThrowIfNull(imageName);

            ImageName = imageName;
            Sequence = sequence;
        }
        public Guid Id { get; set; }
        public Guid ProductId { get; internal set; }
        public Name ImageName { get; private set; }
        public int Sequence { get; private set; }
    }
}
