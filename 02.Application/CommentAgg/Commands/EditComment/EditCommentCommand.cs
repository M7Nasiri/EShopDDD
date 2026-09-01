using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CommentAgg.Commands.EditComment
{
    public record EditCommentCommand(Guid CommentId, string Text) : IBaseCommand;
}
