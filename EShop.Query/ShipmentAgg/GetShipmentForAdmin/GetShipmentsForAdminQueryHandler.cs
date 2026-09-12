using System.Text;
using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.ShipmentAgg.DTOs.Admin;
using EShop.Shared.Query;

namespace EShop.Query.ShipmentAgg.GetShipmentForAdmin;

internal class GetShipmentsForAdminQueryHandler(DapperContext dapperContext)
    : IQueryHandler<GetShipmentsForAdminQuery, ShipmentAdminFilterResult>
{
    public async Task<ShipmentAdminFilterResult> Handle(GetShipmentsForAdminQuery request, CancellationToken cancellationToken)
    {
        var filter = request.FilterParams;
        var dynamicParams = new DynamicParameters();
        var condition = new StringBuilder("WHERE 1=1 ");

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            condition.Append(" AND (s.ReceiverName LIKE @Search OR s.PhoneNumber LIKE @Search OR s.PostalCode LIKE @Search) ");
            dynamicParams.Add("Search", $"%{filter.Search.Trim()}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            condition.Append(" AND s.Status = @Status ");
            dynamicParams.Add("Status", filter.Status.Trim());
        }

        if (!string.IsNullOrWhiteSpace(filter.TrackingCode))
        {
            condition.Append(" AND s.TrackingCode LIKE @TrackingCode ");
            dynamicParams.Add("TrackingCode", $"%{filter.TrackingCode.Trim()}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.Province))
        {
            condition.Append(" AND s.Province = @Province ");
            dynamicParams.Add("Province", filter.Province.Trim());
        }

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            condition.Append(" AND s.City = @City ");
            dynamicParams.Add("City", filter.City.Trim());
        }

        if (filter.OrderId.HasValue && filter.OrderId.Value != Guid.Empty)
        {
            condition.Append(" AND s.OrderId = @OrderId ");
            dynamicParams.Add("OrderId", filter.OrderId.Value);
        }

        using var connection = dapperContext.CreateConnection();

        var countSql = $"SELECT COUNT(1) FROM Shipments s {condition}";
        var totalCount = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(countSql, dynamicParams, cancellationToken: cancellationToken)
        );

        var skip = (filter.PageId - 1) * filter.Take;
        dynamicParams.Add("Skip", skip);
        dynamicParams.Add("Take", filter.Take);

        var sql = $@"
            SELECT 
                s.Id, 
                s.OrderId, 
                s.Status, 
                s.TrackingCode, 
                s.ReceiverName, 
                s.PhoneNumber, 
                s.Province, 
                s.City, 
                s.PostalCode
            FROM Shipments s
            {condition}
            ORDER BY s.Id DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

        var shipments = (await connection.QueryAsync<AdminShipmentDto>(
            new CommandDefinition(sql, dynamicParams, cancellationToken: cancellationToken)
        )).ToList();

        var result = new ShipmentAdminFilterResult();
        result.GeneratePaging(totalCount, filter.Take, filter.PageId);
        result.Data = shipments;

        return result;
    }
}