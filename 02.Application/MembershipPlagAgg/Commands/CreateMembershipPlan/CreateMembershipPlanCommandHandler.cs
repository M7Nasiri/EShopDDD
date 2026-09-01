using _01.Domain.Entities.Aggregates.MembershipPlanAgg;
using _01.Domain.Entities.Aggregates.MembershipPlanAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Persistence;

namespace _02.Application.MembershipPlagAgg.Commands.CreateMembershipPlan
{

    public class CreateMembershipPlanCommandHandler : IBaseCommandHandler<CreateMembershipPlanCommand>
    {
        private readonly IMembershipPlanRepository _membershipPlanRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMembershipPlanCommandHandler(
            IMembershipPlanRepository membershipPlanRepository,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _membershipPlanRepository = membershipPlanRepository;
        }
        public async Task<OperationResult> Handle(CreateMembershipPlanCommand request, CancellationToken cancellationToken)
        {
            if (await _membershipPlanRepository.IsNameUniqueAsync(request.Name, null, cancellationToken))
            {
                throw new EShopDomainException("Plan is exist.");
            }

            var membershipPlan = new MembershipPlan(new Name(request.Name)
                , new Money(request.Price), request.Percent, request.FreeShipping
                , new MembershipDuration(request.DurationInDays));

            _membershipPlanRepository.Add(membershipPlan);

            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success();
        }
    }
}
