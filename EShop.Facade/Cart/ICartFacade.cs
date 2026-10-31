using _01.Domain.Entities.Aggregates.CustomerAgg;
using _02.Application.CartAgg.Commands.AddItem;
using _02.Application.CartAgg.Commands.Clear;
using _02.Application.CartAgg.Commands.MergeGuestCart;
using _02.Application.CartAgg.Commands.RemoveItem;
using EShop.Query.CartAgg.DTOs;
using EShop.Query.CartAgg.GetCartByCustomerId;
using EShop.Shared.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.CartAgg.GetCartByFilter;
using EShop.Query.CartAgg.GetCartByGuestId;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EShop.Facade.Cart
{
    public interface ICartFacade
    {
        //Command
        Task<OperationResult> AddItem(AddItemCommand command);
        Task<OperationResult> Clear(ClearCartCommand command);
        Task<OperationResult> MergeGuestCart(MergeGuestCartCommand command);
        Task<OperationResult> RemoveItem(RemoveItemCommand command);

        //Query
        Task<CartDto?> GetByCustomerId(Guid CustomerId);
        Task<CartFilterResult> GetByFilter(CartFilterParam filterParams);
        Task<CartDto?> GetByGuestId(string GuestId);
    }


    public class CartFacade : ICartFacade
    {
        private readonly IMediator _mediator;

        public CartFacade(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OperationResult> AddItem(AddItemCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Clear(ClearCartCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> MergeGuestCart(MergeGuestCartCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> RemoveItem(RemoveItemCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<CartDto?> GetByCustomerId(Guid CustomerId)
        {
            return await _mediator.Send(new GetCartByCustomerIdQuery(CustomerId));
        }

        public async Task<CartFilterResult> GetByFilter(CartFilterParam filterParams)
        {
            return await _mediator.Send(new GetCartsByFilterQuery(filterParams));
        }

        public async Task<CartDto?> GetByGuestId(string GuestId)
        {
            return await _mediator.Send(new GetCartByGuestIdQuery(GuestId));
        }
    }
}
