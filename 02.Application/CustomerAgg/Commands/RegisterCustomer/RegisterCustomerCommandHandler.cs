using _01.Domain.Entities.Aggregates.CustomerAgg;
using _01.Domain.Entities.Aggregates.CustomerAgg.Repository;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using Microsoft.Extensions.Logging;


namespace _02.Application.CustomerAgg.Commands.RegisterCustomer
{
    public class RegisterCustomerCommandHandler(
        IIdentityService identityService,
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork, 
        ILogger<RegisterCustomerCommandHandler> logger)
        : IBaseCommandHandler<RegisterCustomerCommand>
    {
        public async Task<OperationResult> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            Guid? createdUserId = null;

            
            var fullName =new Name($"{request.Name} {request.Family}".Trim());

            var phoneNumber = new PhoneNumber(
                request.PhoneNumber);

            var email = new Email(
                request.Email);
            try
            {
                createdUserId =
                    await identityService.RegisterCustomerAsync(
                        userName: request.UserName,
                        email: email.Value,
                        phoneNumber: phoneNumber.Value,
                        password: request.Password,
                        firstName: request.Name,
                        lastName: request.Family);

                var customer = new Customer(
                    id: createdUserId.Value,
                    fullName: fullName,
                    phoneNumber: phoneNumber,
                    email: email);

                await customerRepository.AddAsync(
                    customer,
                    cancellationToken);

                await unitOfWork.SaveChangesAsync(
                    cancellationToken);

                return OperationResult.Success();
            }
            catch (Exception originalException)
            {
                if (createdUserId.HasValue)
                {
                    try
                    {
                        await identityService.DeleteUserAsync(
                            createdUserId.Value);
                        logger.LogWarning(
                            originalException,
                            $"ثبت بیزینسی مشتری با خطا مواجه شد. کاربر Identity با شناسه {createdUserId} با موفقیت به‌صورت جبرانی حذف شد.",
                            createdUserId.Value);

                    }
                    catch (Exception compensationException)
                    {
                        logger.LogCritical(
                            compensationException,
                            "خطای بحرانی در فرآیند جبرانی! کاربر در IdentityDb با شناسه {createdUserId} ایجاد شد اما در دیتابیس اصلی ذخیره نشد و حذف جبرانی نیز با شکست روبرو گردید. خطای اصلی: {OriginalError}",
                            createdUserId.Value,
                            originalException.Message);
                    }
                }

                throw;
            }
        }
    }
}
