using System;
using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Provider.Database;
using Microsoft.EntityFrameworkCore;

namespace DigitalStation.Provider.Services;

public class BrandServices : IBrandServices
{
    private readonly dbContext _dbContext;
    public BrandServices(dbContext _dbContext)
    {
        this._dbContext = _dbContext;
    }
    public async Task<List<IdAndNameDto>> GetAsync(int CategoryId)
    {
        try
        {
            var IsValidCategory = await CheckCategoryId(CategoryId);
            if(!IsValidCategory)
            {
                throw new Exception(ResponseCode.InvalidCategoryId);
            }
            var brands = await _dbContext.Brands.Where( x=> x.CategoryId == CategoryId).Select( x=> new IdAndNameDto {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            if(brands.Count <= 0)
            {
                throw new Exception(ResponseCode.NoBrandsExists);
            }
            return brands;
        }
        catch
        {
            throw;
        }
    }
    public async Task<int> AddAsync(AddBrandDto dto)
    {
        try
        {
            bool alreadyExists = await CheckExistingBrand(dto.Name);
            if (alreadyExists)
            {
                throw new Exception(ResponseCode.BrandAlreadyExists);
            }
            bool IsValidCategory = await CheckCategoryId(dto.CategoryId);
            if (!IsValidCategory)
            {
                throw new Exception(ResponseCode.InvalidCategoryId);
            }
            var newBrand = Brand.Mapper(dto);
            await _dbContext.Brands.AddAsync(newBrand);
            await _dbContext.SaveChangesAsync();
            return newBrand.Id;
        }
        catch
        {
            throw;
        }
    }
    public async Task<bool> CheckExistingBrand(string name)
    {
        var brand = await _dbContext.Brands
        .Where(x => x.Name.ToLower() == name.Trim().ToLower() || x.Name.ToUpper() == name.Trim().ToUpper())
            .FirstOrDefaultAsync();

        return brand != null;
    }

    public async Task<bool> CheckCategoryId(int id)
    {
        return await _dbContext.Categories.AnyAsync(x => x.Id == id);
    }
}
