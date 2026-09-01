using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CustomerAgg.Commands.AddCustomerAddress
{
    public sealed record AddCustomerAddressCommand(
     string ReceiverName,
     string PhoneNumber,
     string Title,
     string Province,
     string City,
     string Street,
     string Plaque,
     string PostalCode) : IBaseCommand;
}
