using System;
using DigitalStation.Contract.Dtos;

namespace DigitalStation.Contract.Interfaces;

public interface ICartServices
{
    Task<int> UpsertAsync (AddCartDto dto);
    Task<List<GetCartDto>> GetAsync();
    Task<bool> DeleteAsync(int cartId);
    Task<GetCartDto> GetByIdAsync(int cartId);
}
