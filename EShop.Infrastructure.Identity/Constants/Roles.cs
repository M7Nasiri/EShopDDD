using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Identity.Constants
{
    public static class Roles
    {
        public const string Customer = "Customer";

        public const string Admin = "Admin";

        public const string Moderator = "Moderator";

        public static readonly string[] All =
        [
            Customer,
            Admin,
            Moderator
        ];
    }
}
