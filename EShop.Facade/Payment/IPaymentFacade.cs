using System;
using System.Collections.Generic;
using System.Text;
using _02.Application.PaymentAgg.Commands.InitialPayment;
using _02.Application.PaymentAgg.Commands.ProcessPayment;
using EShop.Query.PaymentAgg.DTOs;
using EShop.Query.PaymentAgg.DTOs.Admin;
using EShop.Query.PaymentAgg.DTOs.Customer;
using EShop.Shared.Application;

namespace EShop.Facade.Payment
{
    public interface IPaymentFacade
    {
        Task<OperationResult<InitiatePaymentResult>> Initial(InitiatePaymentCommand command);

        Task<OperationResult<PaymentCallbackResult>>
            ProcessPaymentCallback(Guid paymentId, string authority, string status);

        Task<OperationResult> Refund(Guid paymentId, string reason);

        Task<AdminPaymentFilterResult> FilterPaymentsForAdmin(AdminPaymentFilterParams filterParams);

        Task<CustomerPaymentFilterResult> GetPaymentsForCustomer(Guid customerId,
            CustomerPaymentFilterParams filterParams);

        Task<PaymentDetailDto?> GetById(Guid paymentId);
        Task<OrderPaymentStatusDto?> GetPaymentStatusOfOrder(Guid orderId);
    }
}
