using System;
using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Provider.Database;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;

namespace DigitalStation.Provider.Services;

public class AddressServices : IAddressServices
{
    private readonly dbContext _dbContext;
    public AddressServices(dbContext _dbContext)
    {
        this._dbContext = _dbContext;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var address = await _dbContext.Addresses.FindAsync(id);
            if(address == null)
            {
                throw new Exception(ResponseCode.InvalidAddressId);
            }
            _dbContext.Remove(address);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            throw;
        }
    }
    public async Task<List<GetAddressDto>> GetAsync(int userId)
    {
        try
        {
            var addresses = await _dbContext.Addresses.Where(x => x.UserId == userId).Select(x => new GetAddressDto
            {
                Id = x.Id,
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
                AlternatePhoneNumber = x.AlternatePhoneNumber,
                Locality = x.Locality,
                Landmark = x.Landmark,
                CompleteAddress = x.CompleteAddress,
                Pincode = x.Pincode.Value,
                City = x.Pincode.City.Name,
                State = x.Pincode.City.Parent.Name
            }).ToListAsync();
            return addresses;
        }
        catch
        {
            throw;
        }
    }

    public async Task<int> UpsertAddressAsync(UpsertAddressDto dto)
    {
        try
        {
            var pincode = await _dbContext.Pincodes.FindAsync(dto.PincodeId);
            if (pincode == null)
            {
                throw new Exception(ResponseCode.InvalidPincodeId);
            }
            var user = await _dbContext.Users.FindAsync(dto.UserId);
            if (user == null)
            {
                throw new Exception(ResponseCode.InvalidUserId);
            }
            if (dto.Id != null && dto.Id != 0)
            {
                var existingAddress = await _dbContext.Addresses.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (existingAddress == null)
                {
                    throw new Exception(ResponseCode.InvalidAddressId);
                }
                Address.Mapper(existingAddress, dto);
                await _dbContext.SaveChangesAsync();
                return existingAddress.Id;
            }
            else
            {
                var newAddress = new Address();
                Address.Mapper(newAddress, dto);
                await _dbContext.Addresses.AddAsync(newAddress);
                await _dbContext.SaveChangesAsync();
                return newAddress.Id;
            }
        }
        catch
        {
            throw;
        }
    }

    public async Task<VerifyPincodeDto?> VerifyPincodeAsync(int pincode)
    {
        try
        {
            var code = await _dbContext.Pincodes.Where(x => x.Value == pincode).Select(x => new VerifyPincodeDto
            {
                PincodeId = x.Id,
                CityName = x.City.Name,
                StateName = x.City.Parent.Name
            }).FirstOrDefaultAsync();
            if (code == null)
            {
                return null;
            }
            return code;
        }
        catch
        {
            throw;
        }
    }
}
