using _01.Domain.Entities.Aggregates.CategoryAgg.Repository;
using _01.Domain.Entities.Aggregates.CustomerAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CustomerAgg.Commands.UpdateCustomerProfile
{
    public class UpdateCustomerProfileCommandHandler : IBaseCommandHandler<UpdateCustomerProfileCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public UpdateCustomerProfileCommandHandler(
             ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }
        public async Task<OperationResult> Handle(UpdateCustomerProfileCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = new Guid(_currentUser.UserId.Value);

            var customer =await  _customerRepository.GetTracking(customerId.Value, cancellationToken);
            if (customer == null)
                throw new EShopNullException("Custoemr was not found.");

            var fullName = new Name(request.Name + " " + request.Family);

            customer.ChangeFullName(fullName);

            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success();
        }
    }
}
