using EShop.Query.CartAgg.DTOs;
using EShop.Shared.Query;

namespace EShop.Query.CartAgg.GetCartByFilter;

public class GetCartsByFilterQuery : QueryFilter<CartFilterResult, CartFilterParam>
{
    public GetCartsByFilterQuery(CartFilterParam filterParams) : base(filterParams)
    {
    }
}