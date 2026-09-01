using _01.Domain.Entities.Aggregates.CustomerAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;

namespace _02.Application.CustomerAgg.Commands.EditCustomerAddress
{
    public class EditCustomerAddressCommandHandler : IBaseCommandHandler<EditCustomerAddressCommand>
    {

        private readonly ICustomerRepository _customerRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        public EditCustomerAddressCommandHandler(ICustomerRepository customerRepository, ICurrentUser currentUser
            , IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> Handle(EditCustomerAddressCommand request, CancellationToken cancellationToken)
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

            var updatedAddress = new Address(
            request.ReceiverName,
            request.PhoneNumber,
            request.Title,
            request.Province,
            request.City,
            request.Street,
            request.Plaque,
            request.PostalCode);

            customer.EditAddress(request.TargetPostalCode, updatedAddress);
            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Success();
        }
    }
}
