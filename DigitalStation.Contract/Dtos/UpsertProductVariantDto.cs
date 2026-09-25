using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalStation.Contract.Dtos;

public class UpsertProductVariantDto
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Info is a required field.")]
    [MinLength(2, ErrorMessage = "Info must be atleast 2 char long.")]
    public string Info { get; set; } = null!;
    [Required(ErrorMessage = "DiscountedPrice is a required field.")]
    [Range(1, int.MaxValue, ErrorMessage = "DiscountedPrice cannot be negative or out of range.")]
    public decimal DiscountedPrice { get; set; }
    [Required(ErrorMessage = "ActualPrice is a required field.")]
    [Range(1, int.MaxValue, ErrorMessage = "ActualPrice cannot be negative or out of range.")]
    public decimal ActualPrice { get; set; }
    [Required(ErrorMessage = "Please add atleast one image of product.")]
    public List<string> Images { get; set; } = null!;
    public string ? FlipkartUrl {get;set;}
    public string ? AmazonUrl {get;set;}
    public bool InStock {get;set;} = true;
    public string ? Highlights {get;set;}
    public int ? ProductId {get;set;}

}
