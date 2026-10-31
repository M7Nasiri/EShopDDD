using _02.Application.CouponAgg.Commands.AddCoupon;
using _02.Application.CouponAgg.Commands.DeleteCoupon;
using _02.Application.CouponAgg.Commands.DisableCoupon;
using _02.Application.CouponAgg.Commands.EnableCoupon;
using EShop.Query.CouponAgg.DTOs;
using EShop.Query.CouponAgg.GetCouponByCode;
using EShop.Shared.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.CouponAgg.GetCouponsForAdmin;
using EShop.Query.CouponAgg.ValidateCoupon;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EShop.Facade.Coupon
{
    public interface ICouponFacade
    {
        //Command
        Task<OperationResult> Add(AddCouponCommand command);
        Task<OperationResult> Delete(DeleteCouponCommand command);
        Task<OperationResult> Disable(DisableCouponCommand command);
        Task<OperationResult> Enable(EnableCouponCommand command);


        //Query
        Task<CouponDetailsDto?> GetByCode(string code);
        Task<CouponFilterResult> GetCouponsForAdmin(CouponFilterParams filterParams);
        Task<ValidateCouponDto> Validate(string code);
    }

    public class CouponFacade : ICouponFacade
    {
        private readonly IMediator _mediator;

        public CouponFacade(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OperationResult> Add(AddCouponCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Delete(DeleteCouponCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Disable(DisableCouponCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Enable(EnableCouponCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<CouponDetailsDto?> GetByCode(string code)
        {
            return await _mediator.Send(new GetCouponByCodeQuery(code));
        }

        public async Task<CouponFilterResult> GetCouponsForAdmin(CouponFilterParams filterParams)
        {
            return await _mediator.Send(new GetCouponsForAdminQuery(filterParams));
        }

        public async Task<ValidateCouponDto> Validate(string code)
        {
            return await _mediator.Send(new ValidateCouponQuery(code));
        }
    }
}
