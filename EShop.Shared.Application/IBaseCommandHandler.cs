using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Shared.Application
{
    public interface IBaseCommandHandler<TCommand> : IRequestHandler<TCommand, OperationResult> where TCommand : IBaseCommand
    {
    }

    public interface IBaseCommandHandler<TCommand, TResponseData> : IRequestHandler<TCommand, OperationResult<TResponseData>> where TCommand : IBaseCommand<TResponseData>
    {
    }
}
