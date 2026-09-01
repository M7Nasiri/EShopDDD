using EShop.Shared.Application;

namespace _02.Application.CartAgg.Commands.RemoveItem
{
    public record RemoveItemCommand(Guid ProductId) : IBaseCommand;

}
