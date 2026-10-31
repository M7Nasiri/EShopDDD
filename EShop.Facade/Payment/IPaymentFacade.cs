using _01.Domain.Entities.Aggregates.CustomerAgg;
using _02.Application.PaymentAgg.Commands.InitialPayment;
using _02.Application.PaymentAgg.Commands.ProcessPayment;
using _02.Application.PaymentAgg.Commands.RefundPayment;
using EShop.Query.PaymentAgg.DTOs;
using EShop.Query.PaymentAgg.DTOs.Admin;
using EShop.Query.PaymentAgg.DTOs.Customer;
using EShop.Query.PaymentAgg.GetAdminPaymentsFilter;
using EShop.Query.PaymentAgg.GetCustomerPayments;
using EShop.Shared.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.PaymentAgg.GetPaymentDetails;
using EShop.Query.PaymentAgg.GetPaymentOrderStatus;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EShop.Facade.Payment
{
    public interface IPaymentFacade
    {
        Task<OperationResult<InitiatePaymentResult>> Initial(InitiatePaymentCommand command);

        Task<OperationResult<PaymentCallbackResult>>
            ProcessPaymentCallback(ProcessPaymentCallbackCommand command);

        Task<OperationResult> Refund(RefundPaymentCommand command);

        Task<AdminPaymentFilterResult> FilterPaymentsForAdmin(AdminPaymentFilterParams filterParams);

        Task<CustomerPaymentFilterResult> GetPaymentsForCustomer(Guid customerId,
            CustomerPaymentFilterParams filterParams);

        Task<PaymentDetailDto?> GetById(Guid paymentId);
        Task<OrderPaymentStatusDto?> GetPaymentStatusOfOrder(Guid orderId);
    }

    public class PaymentFacade : IPaymentFacade
    {
        private readonly IMediator _mediator;

        public PaymentFacade(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OperationResult<InitiatePaymentResult>> Initial(InitiatePaymentCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult<PaymentCallbackResult>> 
            ProcessPaymentCallback(ProcessPaymentCallbackCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Refund(RefundPaymentCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<AdminPaymentFilterResult> FilterPaymentsForAdmin(AdminPaymentFilterParams filterParams)
        {
            return await _mediator.Send(new GetAdminPaymentsFilterQuery(filterParams));
        }

        public async Task<CustomerPaymentFilterResult> GetPaymentsForCustomer(Guid customerId, CustomerPaymentFilterParams filterParams)
        {
            return await _mediator.Send(new GetCustomerPaymentsQuery(customerId, filterParams));
        }

        public async Task<PaymentDetailDto?> GetById(Guid paymentId)
        {
            return await _mediator.Send(new GetPaymentDetailsQuery(paymentId));
        }

        public async Task<OrderPaymentStatusDto?> GetPaymentStatusOfOrder(Guid orderId)
        {
            return await _mediator.Send(new GetOrderPaymentStatusQuery(orderId));
        }
    }
}
