using _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanDiscount;
using _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanStatus;
using _02.Application.MembershipPlagAgg.Commands.CreateMembershipPlan;
using EShop.Query.MembershipPlanAgg.DTOs;
using EShop.Query.MembershipPlanAgg.DTOs.Admin;
using EShop.Query.MembershipPlanAgg.GetActiveMembershipPlan;
using EShop.Query.MembershipPlanAgg.GetAllPlansForAdmin;
using EShop.Query.MembershipPlanAgg.GetMembershipPlanById;
using EShop.Query.MembershipPlanAgg.GetMembershipPlansLookup;
using EShop.Shared.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanPrice;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EShop.Facade.MembershipPlan
{
    public interface IMembershipPlanFacade
    {
        //Commands
        Task<OperationResult> ChangePlanDiscount(ChangeMembershipPlanDiscountCommand command);
        Task<OperationResult> ChangePlanPrice(ChangeMembershipPlanPriceCommand command);
        Task<OperationResult> ChangePlanStatus(ChangeMembershipPlanStatusCommand command);
        Task<OperationResult> Add(CreateMembershipPlanCommand command);

        //Queiris
        Task<IReadOnlyList<MembershipPlanDto>> GetActivePlan();
        Task<MembershipPlanFilterResult> GetAllForAdmin(MembershipPlanFilterParam filterParams);
        Task<MembershipPlanAdminDto?> GetByIdForAdmin(Guid id);
        Task<IReadOnlyList<MembershipPlanLookupDto>> GetPlansLookup();
    }

    public class MembershipPlanFacade : IMembershipPlanFacade
    {
        private readonly IMediator _mediator;

        public MembershipPlanFacade(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<OperationResult> ChangePlanDiscount(ChangeMembershipPlanDiscountCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> ChangePlanPrice(ChangeMembershipPlanPriceCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> ChangePlanStatus(ChangeMembershipPlanStatusCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Add(CreateMembershipPlanCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<IReadOnlyList<MembershipPlanDto>> GetActivePlan()
        {
            return await _mediator.Send(new GetActiveMembershipPlanQuery());
        }

        public async Task<MembershipPlanFilterResult> GetAllForAdmin(MembershipPlanFilterParam filterParams)
        {
            return await _mediator.Send(new GetAllPlansForAdminQuery(filterParams));
        }

        public async Task<MembershipPlanAdminDto?> GetByIdForAdmin(Guid id)
        {
            return await _mediator.Send(new GetMembershipPlanByIdQuery(id));
        }

        public async Task<IReadOnlyList<MembershipPlanLookupDto>> GetPlansLookup()
        {
            return await _mediator.Send(new GetMembershipPlansLookupQuery());
        }
    }
}
