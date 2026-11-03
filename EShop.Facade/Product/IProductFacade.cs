using _02.Application.ProductAgg.Commands.AddImage;
using _02.Application.ProductAgg.Commands.ChangePrice;
using _02.Application.ProductAgg.Commands.CreateProduct;
using _02.Application.ProductAgg.Commands.IncreaseStock;
using _02.Application.ProductAgg.Commands.RemoveImage;
using EShop.Query.ProductAgg.DTOs;
using EShop.Query.ProductAgg.DTOs.Admin;
using EShop.Query.ProductAgg.DTOs.Customer;
using EShop.Query.ProductAgg.DTOs.Details;
using EShop.Shared.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using _02.Application.ProductAgg.Commands.RemoveProduct;
using EShop.Query.ProductAgg.CheckProductStock;
using EShop.Query.ProductAgg.GetProductDetailsById;
using EShop.Query.ProductAgg.GetProductForAdmin;
using EShop.Query.ProductAgg.GetProductsShopFilter;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EShop.Facade.Product
{
    public interface IProductFacade
    {
        //Commands

        Task<OperationResult> AddImage(AddProductImageCommand command);
        Task<OperationResult> ChangePrice(ChangePriceCommand command);
        Task<OperationResult> Add(CreateProductCommand command);
        Task<OperationResult> IncreaseStock(IncreaseStockCommand command);
        Task<OperationResult> RemoveImage(RemoveProductImageCommand command);
        Task<OperationResult> RemoveProduct(RemoveProductCommand command);

        //Queries
        Task<ProductStockDto?> CheckStock(Guid productId, int requiredCount = 1);
        Task<ProductDetailsDto?> GetById(Guid productId);
        Task<ProductAdminFilterResult> GetForAdmin(ProductAdminFilterParams filterParams);
        Task<ShopProductFilterResult> GetForCustomer(ShopProductFilterParams filterParams);
    }

    public class ProductFacade : IProductFacade
    {
        private readonly IMediator _mediator;

        public ProductFacade(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OperationResult> AddImage(AddProductImageCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> ChangePrice(ChangePriceCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Add(CreateProductCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> IncreaseStock(IncreaseStockCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> RemoveImage(RemoveProductImageCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> RemoveProduct(RemoveProductCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<ProductStockDto?> CheckStock(Guid productId, int requiredCount = 1)
        {
            return await _mediator.Send(new CheckProductStockQuery(productId,requiredCount));
        }

        public async Task<ProductDetailsDto?> GetById(Guid productId)
        {
            return await _mediator.Send(new GetProductDetailsByIdQuery(productId));
        }

        public async Task<ProductAdminFilterResult> GetForAdmin(ProductAdminFilterParams filterParams)
        {
            return await _mediator.Send(new GetProductsForAdminQuery(filterParams));
        }

        public async Task<ShopProductFilterResult> GetForCustomer(ShopProductFilterParams filterParams)
        {
            return await _mediator.Send(new GetProductsShopFilterQuery(filterParams));
        }
    }
}
