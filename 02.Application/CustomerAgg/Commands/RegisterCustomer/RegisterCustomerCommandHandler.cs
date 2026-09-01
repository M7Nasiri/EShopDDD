using _01.Domain.Entities.Aggregates.CustomerAgg;
using _01.Domain.Entities.Aggregates.CustomerAgg.Repository;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;

namespace _02.Application.CustomerAgg.Commands.RegisterCustomer
{
    public class RegisterCustomerCommandHandler : IBaseCommandHandler<RegisterCustomerCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly ICustomerRepository _customerRepository;

        public RegisterCustomerCommandHandler(
            IIdentityService identityService,
            ICustomerRepository customerRepository)
        {
            _identityService = identityService;
            _customerRepository = customerRepository;
        }


        public async Task<OperationResult> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var userId = await _identityService.RegisterCustomerAsync(
                request.UserName, request.Email, request.Password, request.Name, request.Family);

            var fullName = $"{request.Name} {request.Family}";
            var customer = new Customer(userId, new Name(fullName));
            await _customerRepository.AddAsync(customer, cancellationToken);
            await _customerRepository.Save();

            return OperationResult.Success();
        }
    }
}
