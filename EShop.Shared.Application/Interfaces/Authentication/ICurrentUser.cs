using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Shared.Application.Interfaces.Authentication
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        Guid? UserId { get; }
        string? GuestId { get; }
        bool IsValid();
        bool IsInRole(string role);
        bool IsAdmin { get; }
        bool CanModerateComments { get; }
    }
}
