using System;
using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Provider.Database;
using Microsoft.EntityFrameworkCore;

namespace DigitalStation.Provider.Services;

public class WishlistServices : IWishlistServices
{
    private readonly dbContext _dbContext;
    private readonly IJwtServices jwtServices;
    public WishlistServices(dbContext _dbContext, IJwtServices jwtServices)
    {
        this._dbContext = _dbContext;
        this.jwtServices = jwtServices;
    }
    public async Task<List<GetWishlistDto>> GetAsync()
    {
        try
        {
            var wishlistItems = await _dbContext.Wishlists.Where(x => x.UserId == jwtServices.Id)
            .Select(x => new GetWishlistDto
            {
                Id = x.Id,
                ProductVariantId = x.ProductVariant.Id,
                ProductId = x.ProductVariant.Product.Id,
                ActualPrice = x.ProductVariant.ActualPrice,
                DiscountedPrice = x.ProductVariant.DiscountedPrice,
                ProductVariantName = x.ProductVariant.Info,
                Images = x.ProductVariant.Images
            }).ToListAsync();
            return wishlistItems;
        }
        catch
        {
            throw;
        }
    }
    public async Task<int> AddAsync(int productVariantId)
    {
        try
        {
            var wishlistItem = new Wishlist
            {
                UserId = jwtServices.Id,
                ProductVariantId = productVariantId
            };
            await _dbContext.Wishlists.AddAsync(wishlistItem);
            await _dbContext.SaveChangesAsync();
            return wishlistItem.Id;
        }
        catch
        {
            throw;
        }
    }
    public async Task<bool> DeleteAsync(int wishlistId)
    {
        try
        {
            var wishlistItem = await _dbContext.Wishlists.FirstOrDefaultAsync(x=> x.Id == wishlistId && x.UserId == jwtServices.Id);
            if(wishlistItem == null)
            {
                throw new Exception(ResponseCode.InvalidWishlistId);
            }
            _dbContext.Wishlists.Remove(wishlistItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            throw;
        }
    }
}
