using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CommentAgg.Commands.AddComment
{
    public record AddCommentCommand(Guid ProductId, string Text) : IBaseCommand;
}
