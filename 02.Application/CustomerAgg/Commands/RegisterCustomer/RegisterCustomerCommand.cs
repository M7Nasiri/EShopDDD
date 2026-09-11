using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CustomerAgg.Commands.RegisterCustomer
{
    public sealed record RegisterCustomerCommand(string UserName, string Name, string Family,
        string Email,string PhoneNumber, string Password) : IBaseCommand;

}
