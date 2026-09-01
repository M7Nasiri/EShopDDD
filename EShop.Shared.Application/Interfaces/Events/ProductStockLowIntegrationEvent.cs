using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Shared.Application.Interfaces.Events
{
    public sealed record ProductStockLowIntegrationEvent(
    Guid ProductId,
    int CurrentStock,
    DateTime OccurredOn);
}
