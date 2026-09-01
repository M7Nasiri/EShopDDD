using _01.Domain.Entities.Aggregates.CustomerAgg.Repository;
using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.SetSaveShippingAddress
{
    public class SetSaveShippingAddressCommandHandler : IBaseCommandHandler<SetSaveShippingAddressCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public SetSaveShippingAddressCommandHandler(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            ICurrentUser currentUser,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult> Handle(SetSaveShippingAddressCommand request, CancellationToken cancellationToken)
        {

            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = _currentUser.UserId.Value;

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(customerId, cancellationToken);

            if (order is null)
                throw new EShopNullException("Order was not found");

            var customer = await _customerRepository.GetWithAddressesTrackingAsync(customerId);

            if (customer == null)
                throw new EShopNullException("Customer was not found.");

            var targetAddress = customer.Addresses.FirstOrDefault(c=>c.PostalCode == request.PostalCode);
            if (targetAddress is null)
                throw new EShopNullException($"Address with this PostalCode{request.PostalCode} was not found.");



            var shippingAddress = new Address(
            targetAddress.ReceiverName,
            targetAddress.PhoneNumber,
            targetAddress.Title,
            targetAddress.Province,
            targetAddress.City,
            targetAddress.Street,
            targetAddress.Plaque,
            targetAddress.PostalCode);
            order.SetShippingAddress(shippingAddress);
            order.SetShippingCost(new Money(request.Cost));
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
    }
}
