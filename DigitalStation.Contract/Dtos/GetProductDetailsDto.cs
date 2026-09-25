using System;
using DigitalStation.Contract.Enums;
using DigitalStation.Contract.Models;

namespace DigitalStation.Contract.Dtos;

public class GetProductDetailsDto
{
    public string Name {get;set;} = null!;
    public string Description{get;set;} = null!;
    public string Specifications{get;set;} = null!;
    public IdAndNameDto Category {get;set;} = null!;
    public IdAndNameDto SubCategory{get;set;} = null!;
    public IdAndNameDto Brand{get;set;} = null!;
    public List<UpsertProductVariantDto> VariantDetails {get;set;} = null!;
}
