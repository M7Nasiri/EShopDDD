using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Shared.Application.Interfaces.Authentication;

public interface IIdentityService
{
    Task<Guid> RegisterCustomerAsync(
        string userName,
        string email,
        string password,
        string? firstName,
        string? lastName);

    Task<Guid?> ValidateUserAsync(
        string userNameOrEmail,
        string password);
}
