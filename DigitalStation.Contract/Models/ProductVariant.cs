using System;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStation.Contract.Dtos;

namespace DigitalStation.Contract.Models;

public class ProductVariant : Base
{
    public int Id { get; set; }
    public Guid ProductVariantGuid { get; set; } = Guid.NewGuid();
    public decimal DiscountedPrice { get; set; }
    public decimal ActualPrice { get; set; }
    public string Info {get;set;} = null!;
    public bool InStock {get;set;}= true;
    public string ? Highlights {get;set;}
    public string ? AmazonUrl {get;set;}
    public string ? FlipkartUrl {get;set;}
    // public int Stock {get;set;}
    // public string? SKU { get; set; }
    public List<string>? Images { get; set; }
    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
    public static ProductVariant Mapper(UpsertProductVariantDto dto)
    {
        return new ProductVariant{
            DiscountedPrice = dto.DiscountedPrice,
            ActualPrice = dto.ActualPrice,
            Info = dto.Info,
            Images = dto.Images,
            FlipkartUrl = dto.FlipkartUrl,
            AmazonUrl = dto.AmazonUrl,
            InStock = dto.InStock,
            Highlights = dto.Highlights
        };
    }
}
