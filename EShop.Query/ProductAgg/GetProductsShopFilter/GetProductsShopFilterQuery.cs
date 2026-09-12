using EShop.Query.ProductAgg.DTOs.Customer;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;

namespace EShop.Query.ProductAgg.GetProductsShopFilter
{
    public class GetProductsShopFilterQuery(ShopProductFilterParams filterParams)
        : QueryFilter<ShopProductFilterResult, ShopProductFilterParams>(filterParams)
    {
    }
}
