using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.CustomerAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace EShop.Query.CustomerAgg.GetCustomerLookupForAdmin
{
    public class GetCustomerLookupForAdminQueryHandler(DapperContext dapperContext)
        : IQueryHandler<GetCustomerLookupForAdminQuery, CustomerFilterResult>
    {

        public async Task<CustomerFilterResult> Handle(GetCustomerLookupForAdminQuery request, CancellationToken cancellationToken)
        {
            var filter = request.FilterParams;
            var dynamicParams = new DynamicParameters();

            var conditions = new StringBuilder("WHERE c.IsDelete = 0");

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                conditions.Append(@" AND (
                    c.FullName LIKE @Search 
                    OR c.Default_PhoneNumber LIKE @Search 
                    OR c.Default_PostalCode LIKE @Search
                )");
                dynamicParams.Add("Search", $"%{filter.Search.Trim()}%");
            }

            if (!string.IsNullOrWhiteSpace(filter.Province))
            {
                conditions.Append(" AND c.Default_Province = @Province");
                dynamicParams.Add("Province", filter.Province.Trim());
            }

            if (!string.IsNullOrWhiteSpace(filter.City))
            {
                conditions.Append(" AND c.Default_City = @City");
                dynamicParams.Add("City", filter.City.Trim());
            }

            using var connection = dapperContext.CreateConnection();
            var countSql = $@"SELECT COUNT(1) FROM Customers c {conditions}";
            var totalCount = await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(countSql, dynamicParams, cancellationToken: cancellationToken)
            );

            var skip = (filter.PageId - 1) * filter.Take;
            dynamicParams.Add("Skip", skip);
            dynamicParams.Add("Take", filter.Take);

            var dataSql = $@"
                SELECT 
                    c.Id,
                    c.FullName,
                    c.Default_ReceiverName AS DefaultReceiverName,
                    c.Default_PhoneNumber AS DefaultPhoneNumber,
                    c.Default_Province AS DefaultProvince,
                    c.Default_City AS DefaultCity,
                    (SELECT COUNT(1) FROM CustomerAddresses ca WHERE ca.CustomerId = c.Id) AS TotalAddresses
                FROM Customers c
                {conditions}
                ORDER BY c.Id DESC
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

            var items = (await connection.QueryAsync<CustomerSummaryDto>(
                new CommandDefinition(dataSql, dynamicParams, cancellationToken: cancellationToken)
            )).AsList();

            var result = new CustomerFilterResult();
            result.GeneratePaging(totalCount, filter.Take, filter.PageId);
            return result;
        }
    }
}
