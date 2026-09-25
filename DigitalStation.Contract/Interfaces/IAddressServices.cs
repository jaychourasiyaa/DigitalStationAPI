using System;
using DigitalStation.Contract.Dtos;

namespace DigitalStation.Contract.Interfaces;

public interface IAddressServices
{
    Task<bool> DeleteAsync(int id);
    Task<int> UpsertAddressAsync(UpsertAddressDto dto);
    Task<List<GetAddressDto>> GetAsync(int userId);
    Task<VerifyPincodeDto?> VerifyPincodeAsync(int pincode);
}
