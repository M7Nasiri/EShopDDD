using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using _02.Application._Utilities;
using EShop.Shared.Application;
using EShop.Shared.Application.FileUtil.Interfaces;

namespace _02.Application.ProductAgg.Commands.RemoveImage
{
    internal class RemoveProductImageCommandHandler : IBaseCommandHandler<RemoveProductImageCommand>
    {
        private readonly IProductRepository _repository;
        private readonly IFileService _fileService;

        public RemoveProductImageCommandHandler(IProductRepository repository, IFileService fileService)
        {
            _repository = repository;
            _fileService = fileService;
        }

        public async Task<OperationResult> Handle(RemoveProductImageCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetTracking(request.ProductId ,cancellationToken);
            if (product == null)
                return OperationResult.NotFound();

            var imageName = product.RemoveImage(request.ImageId);
            await _repository.Save();
            _fileService.DeleteFile(Directories.ProductGalleryImage, imageName);
            return OperationResult.Success();
        }
    }
}