using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CommentAgg.Commands.ApproveComment
{
    public record ApproveCommentCommand(Guid CommentId) : IBaseCommand;
}
