using System;
using DigitalStation.Contract.Dtos;

namespace DigitalStation.Contract.Interfaces;

public interface ICategoryServices
{
    Task<int> AddAsync(AddCategoryDto dto);
    Task<List<IdAndNameDto>> GetRootCategoryAsync();
    Task<List<IdAndNameDto>> GetSubCategoryAsync(int categoryId);
}
