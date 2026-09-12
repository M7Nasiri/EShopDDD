using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.ProductAgg.DTOs.Admin;
using EShop.Shared.Query;

namespace EShop.Query.ProductAgg.GetProductForAdmin
{
    public class GetProductsForAdminQuery(ProductAdminFilterParams filterParams)
        : QueryFilter<ProductAdminFilterResult, ProductAdminFilterParams>(filterParams)
    {
    }
}
