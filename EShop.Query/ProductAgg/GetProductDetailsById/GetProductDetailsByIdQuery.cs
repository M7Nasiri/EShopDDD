using EShop.Query.ProductAgg.DTOs.Details;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.ProductAgg.GetProductDetailsById
{
    public record GetProductDetailsByIdQuery(Guid ProductId) : IQuery<ProductDetailsDto?>;
}
