using System;
using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Provider.Database;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace DigitalStation.Provider.Services;

public class CategoryServices : ICategoryServices
{
    private readonly dbContext _dbContext;
    public CategoryServices(dbContext _dbContext)
    {
        this._dbContext = _dbContext;
    }

    public async Task<int> AddAsync(AddCategoryDto dto)
    {
        try
        {

            var IsValidParentCategory = await _dbContext.Categories.AnyAsync(x => x.Id == dto.ParentCategoryId);
            if (!IsValidParentCategory)
            {
                throw new Exception(ResponseCode.InvalidParentCategoryId);
            }
            var checkExistingCategory = await _dbContext.Categories.Where(x => x.Name.ToLower() == dto.Name.Trim().ToLower()).FirstOrDefaultAsync();
            if (checkExistingCategory != null)
            {
                throw new Exception(ResponseCode.CategoryAlreadyExists);
            }
            var newCategory = Category.Mapper(dto);
            await _dbContext.Categories.AddAsync(newCategory);
            await _dbContext.SaveChangesAsync();
            return newCategory.Id;
        }
        catch
        {
            throw;
        }
    }

    public async Task<List<IdAndNameDto>> GetRootCategoryAsync()
    {
        try
        {
            var categories = await _dbContext.Categories
            .Where(x => x.IsActive && x.ParentId == null)
            .Select(x =>
            new IdAndNameDto
            {
                Id = x.Id,
                Name = x.Name,
                IconUrl = x.IconUrl
            }).ToListAsync();
            return categories;
        }
        catch
        {
            throw;
        }
    }

    public async Task<List<IdAndNameDto>> GetSubCategoryAsync(int categoryId)
    {
        try
        {
            var subcategories = await _dbContext.Categories.Where( x=> x.ParentId == categoryId && x.IsActive).
            Select(x => new IdAndNameDto{
                Id = x.Id,
                Name = x.Name,
                IconUrl = x.IconUrl
            }).ToListAsync();
            return subcategories;
        }
        catch
        {
            throw;
        }
    }
}
