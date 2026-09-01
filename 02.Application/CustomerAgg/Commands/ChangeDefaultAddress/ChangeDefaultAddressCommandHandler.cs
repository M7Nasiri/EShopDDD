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

namespace _02.Application.CustomerAgg.Commands.ChangeDefaultAddress
{
    public class ChangeDefaultAddressCommandHandler : IBaseCommandHandler<ChangeDefaultAddressCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeDefaultAddressCommandHandler(ICustomerRepository customerRepository, ICurrentUser currentUser
             , IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> Handle(ChangeDefaultAddressCommand request, CancellationToken cancellationToken)
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

            customer.ChangeDefaultAddress(request.PostalCode);

            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success();
        }
        
    }
}
