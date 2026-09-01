using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CustomerAgg.Commands.UpdateCustomerProfile
{
    public sealed record UpdateCustomerProfileCommand(string Name, string Family) : IBaseCommand;
}
