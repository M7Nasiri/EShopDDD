using EShop.Shared.Application;
using Microsoft.AspNetCore.Http;

namespace _02.Application.ProductAgg.Commands.AddImage
{
    public class AddProductImageCommand : IBaseCommand
    {

        public IFormFile ImageFile { get; set; }
        public Guid ProductId { get; set; }
        public int Sequence { get; set; }
    }
}