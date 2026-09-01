using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CustomerAgg.Commands.EditCustomerAddress
{
    public sealed record EditCustomerAddressCommand(
     string ReceiverName,
     string PhoneNumber,
     string TargetPostalCode,
     string Title,
     string Province,
     string City,
     string Street,
     string Plaque,
     string PostalCode) : IBaseCommand;
}
