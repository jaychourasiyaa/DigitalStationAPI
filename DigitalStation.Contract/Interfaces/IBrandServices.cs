using System;
using DigitalStation.Contract.Dtos;

namespace DigitalStation.Contract.Interfaces;

public interface IBrandServices
{
    Task <int> AddAsync(AddBrandDto dto);
    Task<List<IdAndNameDto>> GetAsync(int CategoryId);

}
