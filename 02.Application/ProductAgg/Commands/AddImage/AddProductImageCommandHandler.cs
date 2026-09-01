using _01.Domain.Entities.Aggregates.ProductAgg;
using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using _01.Domain.ValueObjects;
using _02.Application._Utilities;
using EShop.Shared.Application;
using EShop.Shared.Application.FileUtil.Interfaces;

namespace _02.Application.ProductAgg.Commands.AddImage
{
    internal class AddProductImageCommandHandler : IBaseCommandHandler<AddProductImageCommand>
    {
        private readonly IProductRepository _repository;
        private readonly IFileService _fileService;

        public AddProductImageCommandHandler(IProductRepository repository, IFileService fileService)
        {
            _repository = repository;
            _fileService = fileService;
        }

        public async Task<OperationResult> Handle(AddProductImageCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetTracking(request.ProductId,cancellationToken);
            if (product == null)
                return OperationResult.NotFound();

            var imageName = await _fileService
                .SaveFileAndGenerateName(request.ImageFile, Directories.ProductGalleryImage);

            var productImage = new ProductImage(new Name(imageName), request.Sequence);
            product.AddImage(productImage);
            await _repository.Save();
            return OperationResult.Success();
        }
    }
}