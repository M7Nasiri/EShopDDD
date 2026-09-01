using _01.Domain.Entities.Aggregates.ShipmentAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ShipmentAgg.Commands.DeliverShip
{
    internal class DeliverShipmentCommandHandler : IBaseCommandHandler<DeliverShipmentCommand>
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeliverShipmentCommandHandler(IShipmentRepository shipmentRepository, IUnitOfWork unitOfWork)
        {
            _shipmentRepository = shipmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> Handle(DeliverShipmentCommand request, CancellationToken cancellationToken)
        {
            var shipment = await _shipmentRepository.GetTracking(request.ShipmentId, cancellationToken);
            if (shipment is null)
                throw new EShopDomainException("مرسوله مورد نظر یافت نشد.");

            shipment.MarkAsDelivered();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult.Success();
        }
    }
}
