using EShop.Shared.Application;
using Microsoft.AspNetCore.Http;

namespace _02.Application.ProductAgg.Commands.AddImage
{
    public class AddProductImageCommand : IBaseCommand
    {
        public AddProductImageCommand(Guid productId,IFormFile imageFile,int sequence)
        {
            ProductId = productId;
            ImageFile = imageFile;
            Sequence = sequence;
        }

        public IFormFile ImageFile { get; set; }
        public Guid ProductId { get; set; }
        public int Sequence { get; set; }
    }
}