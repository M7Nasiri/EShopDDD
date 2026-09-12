using EShop.Query.PaymentAgg.DTOs;
using EShop.Shared.Application;
using EShop.Shared.Query;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using EShop.Infrastructure.Persistent.Dapper;

namespace EShop.Query.PaymentAgg.GetPaymentDetails
{
    public class GetPaymentDetailsQueryHandler(DapperContext context) : IQueryHandler<GetPaymentDetailsQuery,PaymentDetailDto?>
    {
        public async Task<PaymentDetailDto?> Handle(GetPaymentDetailsQuery request, CancellationToken cancellationToken)
        {
            using var connection = context.CreateConnection();
            const string sql = @"SELECT Id AS PaymentId, OrderId, Amount, Method, Status, 
                                    GatewayTransactionId, RefundTransactionId, CreatedAt, Authority 
                             FROM Payments WHERE Id = @PaymentId";

            var payment = await connection.QueryFirstOrDefaultAsync<PaymentDetailDto>(sql, new { request.PaymentId });

            return payment;
        }
    }
}
