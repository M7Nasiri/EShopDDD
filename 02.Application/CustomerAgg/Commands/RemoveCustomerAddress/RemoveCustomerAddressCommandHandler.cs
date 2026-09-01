using _01.Domain.Entities.Aggregates.CustomerAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CustomerAgg.Commands.RemoveCustomerAddress
{
    public class RemoveCustomerAddressCommandHandler : IBaseCommandHandler<RemoveCustomerAddressCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        public RemoveCustomerAddressCommandHandler(ICustomerRepository customerRepository, ICurrentUser currentUser
            , IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult> Handle(RemoveCustomerAddressCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var userId = _currentUser.UserId.Value;

            var customer = await _customerRepository.GetWithAddressesTrackingAsync(userId, cancellationToken);
            if (customer == null)
                throw new EShopNullException("Customer was not found.");

            customer.RemoveAddress(request.PostalCode);

            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Success();
        }
    }
}
