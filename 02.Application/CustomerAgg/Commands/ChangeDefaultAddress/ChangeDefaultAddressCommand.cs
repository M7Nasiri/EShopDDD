using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CustomerAgg.Commands.ChangeDefaultAddress
{
    public sealed record ChangeDefaultAddressCommand(string PostalCode) : IBaseCommand;
}
