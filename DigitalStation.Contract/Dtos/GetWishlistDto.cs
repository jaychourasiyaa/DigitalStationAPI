using System;

namespace DigitalStation.Contract.Dtos;

public class GetWishlistDto
{
    public int Id {get;set;}
    public int ProductId {get;set;}
    public string ProductVariantName {get;set;} = null!;
    public decimal DiscountedPrice { get; set; }
    public decimal ActualPrice { get; set; }
    public List<string> ? Images {get;set;}
    public int ProductVariantId {get;set;}
}
