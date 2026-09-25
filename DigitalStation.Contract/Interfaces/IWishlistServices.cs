using System;
using DigitalStation.Contract.Dtos;

namespace DigitalStation.Contract.Interfaces;

public interface IWishlistServices
{
    Task<int> AddAsync (int productVariantId);
    Task<List<GetWishlistDto>> GetAsync();
    Task<bool> DeleteAsync(int wishlistId);
}
