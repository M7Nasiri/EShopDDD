using _01.Domain.Entities.Aggregates.CustomerAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CustomerAgg.Commands.AddCustomerAddress
{
    internal class AddCustomerAddressCommandHandler : IBaseCommandHandler<AddCustomerAddressCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        public AddCustomerAddressCommandHandler(ICustomerRepository customerRepository, ICurrentUser currentUser
            ,IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> Handle(AddCustomerAddressCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = new Guid(_currentUser.UserId.Value);

            var customer = await _customerRepository.GetWithAddressesTrackingAsync(
                customerId, cancellationToken);

            if (customer is null)
                throw new EShopNullException("پروفایل مشتری یافت نشد.");

            var address = new Address(
                request.ReceiverName,
                request.PhoneNumber,
                request.Title,
                request.Province,
                request.City,
                request.Street,
                request.Plaque,
                request.PostalCode);

            customer.AddAddress(address);

            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Success();
        }
    }
}
