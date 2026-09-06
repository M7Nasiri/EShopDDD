using EShop.Shared.Query.Filter;
using MediatR;

namespace EShop.Shared.Query;

public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : class?
{
}

public class QueryFilter<TResponse, TParam> : IQuery<TResponse>
    where TResponse : BaseFilter
    where TParam : BaseFilterParam
{
    public TParam FilterParams { get; set; }
    public QueryFilter(TParam filterParams)
    {
        FilterParams = filterParams;
    }
}