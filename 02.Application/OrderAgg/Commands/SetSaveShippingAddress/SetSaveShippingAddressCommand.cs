using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.SetSaveShippingAddress
{
    public sealed record SetSaveShippingAddressCommand(string PostalCode,decimal Cost) : IBaseCommand;
}
