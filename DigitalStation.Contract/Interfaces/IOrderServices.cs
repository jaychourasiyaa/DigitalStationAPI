using System;
using DigitalStation.Contract.Dtos;

namespace DigitalStation.Contract.Interfaces;

public interface IOrderServices
{
    Task<int> AddAsync(AddOrderDto dto);
}
