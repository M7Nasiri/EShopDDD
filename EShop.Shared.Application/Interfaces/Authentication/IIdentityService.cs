using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace EShop.Shared.Application.Interfaces.Authentication;

public interface IIdentityService
{
    Task<Guid> RegisterCustomerAsync(
        string userName,
        string email,
        string phoneNumber,
        string password,
        string? firstName,
        string? lastName);

    Task DeleteUserAsync(Guid userId);

    Task<Guid?> ValidateUserAsync(
        string userNameOrEmail,
        string password);
}
