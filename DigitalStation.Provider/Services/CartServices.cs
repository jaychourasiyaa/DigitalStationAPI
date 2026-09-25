using System;
using System.Runtime.ExceptionServices;
using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Provider.Database;
using Azure;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace DigitalStation.Provider.Services;

public class CartServices : ICartServices
{
    private readonly dbContext _dbContext;
    private readonly IJwtServices jwtServices;
    public CartServices(dbContext _dbContext, IJwtServices jwtServices)
    {
        this._dbContext = _dbContext;
        this.jwtServices = jwtServices;
    }
    public async Task<int> UpsertAsync(AddCartDto dto)
    {
        try
        {
            var productVariant = await _dbContext.ProductVariants.FindAsync(dto.ProductVariantId);
            if (productVariant == null)
            {
                throw new Exception(ResponseCode.InvalidProductVariantId);
            }
            Cart ? newCartItem = null;
            var existingCartItem = await _dbContext.Carts.FirstOrDefaultAsync(x => x.ProductVariantId == dto.ProductVariantId && x.UserId == jwtServices.Id);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity = dto.Quantity;
            }
            else
            {
                newCartItem = new Cart
                {
                    UserId = jwtServices.Id,
                    ProductVariantId = dto.ProductVariantId,
                    Quantity = dto.Quantity
                };
                await _dbContext.Carts.AddAsync(newCartItem);
            }
            await _dbContext.SaveChangesAsync();
            return existingCartItem != null ? existingCartItem.Id : ( newCartItem != null ? newCartItem.Id : 0);
        }
        catch
        {
            throw;
        }
    }
    public async Task<List<GetCartDto>> GetAsync()
    {
        try
        {
            var cartItems = await _dbContext.Carts.Where(x => x.UserId == jwtServices.Id)
            .Select(x => new GetCartDto
            {
                Id = x.Id,
                ProductId = x.ProductVariant.Product.Id,
                ProductVariantId = x.ProductVariant.Id,
                ActualPrice = x.ProductVariant.ActualPrice,
                DiscountedPrice = x.ProductVariant.DiscountedPrice,
                Quantity = x.Quantity,
                ProductVariantName = x.ProductVariant.Info,
                Images = x.ProductVariant.Images
            }).ToListAsync();
            return cartItems;
        }
        catch
        {
            throw;
        }
    }
    public async Task<GetCartDto> GetByIdAsync(int cartId)
    {
        try
        {
            var cartItem = await _dbContext.Carts.Where(x => x.Id == cartId)
            .Select(x => new GetCartDto
            {
                Id = x.Id,
                ProductId = x.ProductVariant.Product.Id,
                ProductVariantId = x.ProductVariant.Id,
                ActualPrice = x.ProductVariant.ActualPrice,
                DiscountedPrice = x.ProductVariant.DiscountedPrice,
                Quantity = x.Quantity,
                ProductVariantName = x.ProductVariant.Info,
                Images = x.ProductVariant.Images
            }).FirstOrDefaultAsync();
            if(cartItem == null)
            {
                throw new Exception(ResponseCode.InvalidCartId);
            }
            return cartItem;
        }
        catch
        {
            throw;
        }
    }
    public async Task<bool> DeleteAsync(int cartId)
    {
        try
        {
            var cartItem = await _dbContext.Carts.FirstOrDefaultAsync(x => x.Id == cartId && x.UserId == jwtServices.Id);
            if (cartItem == null)
            {
                throw new Exception(ResponseCode.InvalidCartId);
            }
            _dbContext.Carts.Remove(cartItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            throw;
        }
    }
}
