using _02.Application.MembershipAgg.Commands.CancellMembership;
using _02.Application.MembershipAgg.Commands.ExpireMembership;
using _02.Application.MembershipAgg.Commands.PurchaseMembership;
using EShop.Query.MembershipAgg.DTOs;
using EShop.Shared.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.MembershipAgg.GetCustomerActiveMembership;
using EShop.Query.MembershipAgg.GetCustomerMembershipsHistory;
using EShop.Query.MembershipAgg.GetMembershipById;
using EShop.Query.MembershipAgg.GetMembershipForAdmin;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EShop.Facade.Membership
{
    public interface IMembershipFacade
    {
        //Commands
        Task<OperationResult> Cancell(CancellMembershipCommand command);
        Task<OperationResult> Expire(ExpireMembershipCommand command);
        Task<OperationResult> Purchase(PurchaseMembershipCommand command);


        //Queries
        Task<ActiveMembershipDto?> GetActiveMembership(Guid customerId);
        Task<IReadOnlyList<CustomerMembershipHistoryDto>> GetMembershipHistory(Guid customerId);
        Task<MembershipAdminDto?> GetById(Guid id);
        Task<MembershipFilterResult> GetForAdmin(MembershipFilterParam filterParams);

    }

    public class MembershipFacade : IMembershipFacade
    {
        private readonly IMediator _mediator;

        public MembershipFacade(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OperationResult> Cancell(CancellMembershipCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Expire(ExpireMembershipCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Purchase(PurchaseMembershipCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<ActiveMembershipDto?> GetActiveMembership(Guid customerId)
        {
            return await _mediator.Send(new GetCustomerActiveMembershipQuery(customerId));
        }

        public async Task<IReadOnlyList<CustomerMembershipHistoryDto>> GetMembershipHistory(Guid customerId)
        {
            return await _mediator.Send(new GetCustomerMembershipsHistoryQuery(customerId));
        }

        public async Task<MembershipAdminDto?> GetById(Guid id)
        {
            return await _mediator.Send(new GetMembershipByIdQuery(id));
        }

        public async Task<MembershipFilterResult> GetForAdmin(MembershipFilterParam filterParams)
        {
            return await _mediator.Send(new GetMembershipsForAdminQuery(filterParams));
        }
    }
}
