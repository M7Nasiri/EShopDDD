using _02.Application.CustomerAgg.Commands.AddCustomerAddress;
using _02.Application.CustomerAgg.Commands.ChangeDefaultAddress;
using _02.Application.CustomerAgg.Commands.EditCustomerAddress;
using _02.Application.CustomerAgg.Commands.RegisterCustomer;
using _02.Application.CustomerAgg.Commands.RemoveCustomerAddress;
using _02.Application.CustomerAgg.Commands.UpdateCustomerProfile;
using EShop.Query.CustomerAgg.DTOs;
using EShop.Query.CustomerAgg.GetAddressesOfCustomer;
using EShop.Query.CustomerAgg.GetCustomerLookupForAdmin;
using EShop.Query.CustomerAgg.GetDetailsOfCustomerForAdmin;
using EShop.Shared.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.CustomerAgg.GetProfileOfCustomer;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EShop.Facade.Customer
{
    public interface ICustomerFacade
    {
        //Command
        Task<OperationResult> AddAddress(AddCustomerAddressCommand command);
        Task<OperationResult> ChangeDefaultAddress(ChangeDefaultAddressCommand command);
        Task<OperationResult> EditAddress(EditCustomerAddressCommand command);
        Task<OperationResult> RemoveAddress(RemoveCustomerAddressCommand command);
        Task<OperationResult> Register(RegisterCustomerCommand command);
        Task<OperationResult> UpdateProfile(UpdateCustomerProfileCommand command);

        //Query
        Task<IReadOnlyList<AddressDto>> GetAddresses(Guid customerId);
        Task<CustomerFilterResult> GetCustomerLookupForAdmin(CustomerFilterParams filterParams);
        Task<CustomerDetailsForAdminDto?> GetCustomerDetailsForAdmin(Guid customerId);
        Task<CustomerProfileDto> GetProfile(Guid customerId);
    }

    public class CustomerFacade : ICustomerFacade
    {
        private readonly IMediator _mediator;

        public CustomerFacade(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OperationResult> AddAddress(AddCustomerAddressCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> ChangeDefaultAddress(ChangeDefaultAddressCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> EditAddress(EditCustomerAddressCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> RemoveAddress(RemoveCustomerAddressCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Register(RegisterCustomerCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> UpdateProfile(UpdateCustomerProfileCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<IReadOnlyList<AddressDto>> GetAddresses(Guid customerId)
        {
            return await _mediator.Send(new GetAddressesOfCustomerQuery(customerId));
        }

        public async Task<CustomerFilterResult> GetCustomerLookupForAdmin(CustomerFilterParams filterParams)
        {
            return await _mediator.Send(new GetCustomerLookupForAdminQuery(filterParams));
        }

        public async Task<CustomerDetailsForAdminDto?> GetCustomerDetailsForAdmin(Guid customerId)
        {
            return await _mediator.Send(new GetDetailsOfCustomerForAdminQuery(customerId));
        }

        public async Task<CustomerProfileDto> GetProfile(Guid customerId)
        {
            return await _mediator.Send(new GetProfileOfCustomerQuery(customerId));
        }
    }
}
