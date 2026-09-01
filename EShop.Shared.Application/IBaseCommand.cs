using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Shared.Application
{
    public interface IBaseCommand : IRequest<OperationResult>
    {
    }

    public interface IBaseCommand<TData> : IRequest<OperationResult<TData>>
    {
    }
}
