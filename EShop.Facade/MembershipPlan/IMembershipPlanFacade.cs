using _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanDiscount;
using _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanStatus;
using _02.Application.MembershipPlagAgg.Commands.CreateMembershipPlan;
using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.MembershipPlanAgg.DTOs;
using EShop.Query.MembershipPlanAgg.DTOs.Admin;

namespace EShop.Facade.MembershipPlan
{
    public interface IMembershipPlanFacade
    {
        //Commands
        Task<OperationResult> ChangePlanDiscount(ChangeMembershipPlanPriceCommand command);
        Task<OperationResult> ChangePlanPrice(ChangeMembershipPlanPriceCommand command);
        Task<OperationResult> ChangePlanStatus(ChangeMembershipPlanStatusCommand command);
        Task<OperationResult> Add(CreateMembershipPlanCommand command);

        //Queiris
        Task<IReadOnlyList<MembershipPlanDto>> GetActivePlan();
        Task<MembershipPlanFilterResult> GetAllForAdmin(MembershipPlanFilterParam filterParams);
        Task<MembershipPlanAdminDto?> GetByIdForAdmin(Guid id);
        Task<IReadOnlyList<MembershipPlanLookupDto>> GetPlansLookup();
    }
}
