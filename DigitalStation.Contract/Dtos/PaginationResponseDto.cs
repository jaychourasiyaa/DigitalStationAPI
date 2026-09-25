using System;

namespace DigitalStation.Contract.Dtos;

public class PaginationResponseDto<T>
{
    public int Count {get;set;}
    public int Pages {get;set;}
    public T ? Data {get;set;}
}