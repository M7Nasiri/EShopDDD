using _01.Domain.Entities.Aggregates.OrderAgg;
using _02.Application.ShipmentAgg.Commands.DeliverShip;
using _02.Application.ShipmentAgg.Commands.ReturnShipment;
using _02.Application.ShipmentAgg.Commands.ShipOrder;
using EShop.Query.ShipmentAgg.DTOs;
using EShop.Query.ShipmentAgg.DTOs.Admin;
using EShop.Query.ShipmentAgg.GetShipmentByOrderId;
using EShop.Shared.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.ShipmentAgg.GetShipmentDetailsForAdmin;
using EShop.Query.ShipmentAgg.GetShipmentForAdmin;
using EShop.Query.ShipmentAgg.TrackShipmentByTrackingCode;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EShop.Facade.Shipment
{
    public interface IShipmentFacade
    {
        //Commands

        Task<OperationResult> Deliver(DeliverShipmentCommand command);
        Task<OperationResult> Return(ReturnShipmentCommand command);
        Task<OperationResult> Ship(ShipOrderCommand command);

        //Queries
        Task<OrderShipmentDto?> GetByOrderId(Guid orderId);
        Task<AdminShipmentDetailsDto?> GetById(Guid shipmentId);
        Task<ShipmentAdminFilterResult> GetAllForAdmin(ShipmentAdminFilterParams filterParams);
        Task<TrackingShipmentResultDto?> TrackByTrackingCode(string trackingCode);
    }

    public class ShipmentFacade : IShipmentFacade
    {
        private readonly IMediator _mediator;

        public ShipmentFacade(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OperationResult> Deliver(DeliverShipmentCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Return(ReturnShipmentCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Ship(ShipOrderCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OrderShipmentDto?> GetByOrderId(Guid orderId)
        {
            return await _mediator.Send(new GetShipmentByOrderIdQuery(orderId));
        }

        public async Task<AdminShipmentDetailsDto?> GetById(Guid shipmentId)
        {
            return await _mediator.Send(new GetShipmentDetailsForAdminQuery(shipmentId));
        }

        public async Task<ShipmentAdminFilterResult> GetAllForAdmin(ShipmentAdminFilterParams filterParams)
        {
            return await _mediator.Send(new GetShipmentsForAdminQuery(filterParams));
        }

        public async Task<TrackingShipmentResultDto?> TrackByTrackingCode(string trackingCode)
        {
            return await _mediator.Send(new TrackShipmentByTrackingCodeQuery(trackingCode));
        }
    }
}
