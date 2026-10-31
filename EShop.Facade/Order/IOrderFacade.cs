using _02.Application.OrderAgg.Commands.AddItemToOrder;
using _02.Application.OrderAgg.Commands.ApplyCouponToOrder;
using _02.Application.OrderAgg.Commands.ApplyMembershipDiscount;
using _02.Application.OrderAgg.Commands.ChangeOrderItemQuantity;
using _02.Application.OrderAgg.Commands.CheckoutOrder;
using _02.Application.OrderAgg.Commands.OrderLogistics;
using _02.Application.OrderAgg.Commands.ProcessOrderPaymentCallback;
using _02.Application.OrderAgg.Commands.RemoveCouponFromOrder;
using _02.Application.OrderAgg.Commands.RemoveItemFromOrder;
using _02.Application.OrderAgg.Commands.RemoveMembershipDiscount;
using _02.Application.OrderAgg.Commands.SetSaveShippingAddress;
using EShop.Query.OrderAgg.DTOs;
using EShop.Query.OrderAgg.DTOs.GetActiveDraftOrder;
using EShop.Query.OrderAgg.DTOs.GetAdminOrderDetails;
using EShop.Query.OrderAgg.DTOs.GetCustomerOrderDetails;
using EShop.Shared.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.OrderAgg.GetActiveDraftOrder;
using EShop.Query.OrderAgg.GetAdminOrderDetails;
using EShop.Query.OrderAgg.GetCustomerOrderDetails;
using EShop.Query.OrderAgg.GetCustomerOrders;
using EShop.Query.OrderAgg.GetCustomersOrdersForAdmin;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EShop.Facade.Order
{
    public interface IOrderFacade
    {
        Task<OperationResult> AddItem(AddItemToOrderCommand command);
        Task<OperationResult> ApplyCoupon(ApplyCouponToOrderCommand command);
        Task<OperationResult> ApplyMembershipDiscount(ApplyMembershipDiscountCommand command);
        Task<OperationResult> ChangeQuantityOfItem(ChangeOrderItemQuantityCommand command);
        Task<OperationResult<CheckoutOrderResult>> Checkout(CheckoutOrderCommand command);
        Task<OperationResult> Logistics(ShipOrderCommand command);
        Task<OperationResult> ProcessPaymentCallback(ProcessOrderPaymentCallbackCommand command);
        Task<OperationResult> RemoveCoupon(RemoveCouponFromOrderCommand command);
        Task<OperationResult> RemoveItem(RemoveItemFromOrderCommand command);
        Task<OperationResult> RemoveMembershipDiscount(RemoveMembershipDiscountCommand command);
        Task<OperationResult> SetSaveShippingAddress(SetSaveShippingAddressCommand command);


        //Queris
        Task<DraftOrderDto?> GetActiveDraft(Guid customerId);
        Task<AdminOrderDetailDto?> GetDetailOfOrderForAdmin(Guid orderId);
        Task<CustomerOrderDetailDto?> GetDetailOfOrderForCustomer(Guid orderId, Guid customerId);
        Task<CustomerOrdersFilterResult> GetCustomerOrderList(CustomerOrdersFilterParams filterParams);
        Task<CustomersOrdersForAdminFilterResult> GetCustomerOrderListForAdmin
            (CustomersOrdersForAdminFilterParams filterParams);


    }
    public class OrderFacade : IOrderFacade
    {
        private readonly IMediator _mediator;

        public OrderFacade(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OperationResult> AddItem(AddItemToOrderCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> ApplyCoupon(ApplyCouponToOrderCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> ApplyMembershipDiscount(ApplyMembershipDiscountCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> ChangeQuantityOfItem(ChangeOrderItemQuantityCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult<CheckoutOrderResult>> Checkout(CheckoutOrderCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Logistics(ShipOrderCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> ProcessPaymentCallback(ProcessOrderPaymentCallbackCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> RemoveCoupon(RemoveCouponFromOrderCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> RemoveItem(RemoveItemFromOrderCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> RemoveMembershipDiscount(RemoveMembershipDiscountCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> SetSaveShippingAddress(SetSaveShippingAddressCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<DraftOrderDto?> GetActiveDraft(Guid customerId)
        {
            return await _mediator.Send(new GetActiveDraftOrderQuery(customerId));
        }

        public async Task<AdminOrderDetailDto?> GetDetailOfOrderForAdmin(Guid orderId)
        {
            return await _mediator.Send(new GetAdminOrderDetailsQuery(orderId));
        }

        public async Task<CustomerOrderDetailDto?> GetDetailOfOrderForCustomer(Guid orderId, Guid customerId)
        {
            return await _mediator.Send(new GetCustomerOrderDetailsQuery(orderId,customerId));
        }

        public async Task<CustomerOrdersFilterResult> GetCustomerOrderList(CustomerOrdersFilterParams filterParams)
        {
            return await _mediator.Send(new GetCustomerOrdersQuery(filterParams));
        }

        public async Task<CustomersOrdersForAdminFilterResult> GetCustomerOrderListForAdmin(CustomersOrdersForAdminFilterParams filterParams)
        {
            return await _mediator.Send(new GetCustomersOrdersForAdminQuery(filterParams));
        }
    }
}
