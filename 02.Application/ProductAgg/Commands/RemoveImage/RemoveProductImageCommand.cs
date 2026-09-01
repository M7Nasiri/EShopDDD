using EShop.Shared.Application;

namespace _02.Application.ProductAgg.Commands.RemoveImage
{
    public record RemoveProductImageCommand(Guid ProductId, Guid ImageId) : IBaseCommand;
}