using System;

namespace DigitalStation.Contract.Dtos;

public class GetProductVariantDto
{
    public int ProductId {get;set;}
    public string ProductVariantName {get;set;} = null!;
    public decimal DiscountedPrice { get; set; }
    public decimal ActualPrice { get; set; }
    public List<string> ? Images {get;set;}
}
