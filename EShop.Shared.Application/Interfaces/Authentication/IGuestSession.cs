using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Shared.Application.Interfaces.Authentication
{
    public interface IGuestSession
    {
        string GetOrCreateGuestId();
    }
}
