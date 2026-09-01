using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CustomerAgg.Commands.RemoveCustomerAddress
{
    public sealed record RemoveCustomerAddressCommand(string PostalCode) : IBaseCommand;
}
