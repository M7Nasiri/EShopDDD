using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CommentAgg.Commands.DeleteComment
{
    public sealed record DeleteCommentCommand(Guid CommentId) : IBaseCommand;
}
