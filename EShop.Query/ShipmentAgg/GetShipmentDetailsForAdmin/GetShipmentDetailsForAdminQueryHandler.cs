using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.ShipmentAgg.DTOs;
using EShop.Shared.Query;

namespace EShop.Query.ShipmentAgg.GetShipmentDetailsForAdmin;

internal class GetShipmentDetailsForAdminQueryHandler(DapperContext dapperContext)
    : IQueryHandler<GetShipmentDetailsForAdminQuery, AdminShipmentDetailsDto?>
{
    public async Task<AdminShipmentDetailsDto?> Handle(GetShipmentDetailsForAdminQuery request, CancellationToken cancellationToken)
    {
        using var connection = dapperContext.CreateConnection();

        const string sql = @"
            SELECT 
                s.Id, 
                s.OrderId, 
                s.Status, 
                s.TrackingCode, 
                s.Title AS AddressTitle,
                s.ReceiverName, 
                s.PhoneNumber, 
                s.Province, 
                s.City, 
                s.Street, 
                s.PostalCode, 
                s.Plaque
            FROM Shipments s
            WHERE s.Id = @ShipmentId;";

        return await connection.QueryFirstOrDefaultAsync<AdminShipmentDetailsDto>(
            new CommandDefinition(sql, new { ShipmentId = request.ShipmentId }, cancellationToken: cancellationToken)
        );
    }
}