using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalStation.Contract.Dtos;

public class UpsertProductDto
{
    public int ? Id {get;set;}
    [Required(ErrorMessage = "Name is a required field.")]
    [MinLength(2, ErrorMessage = "Name must be atleast 2 char long.")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Description is a required field.")]
    [MinLength(2, ErrorMessage = "Description must be atleast 50 char long.")]
    public string? Description { get; set; }
    [Required(ErrorMessage = "Specifications is a required field.")]
    [MinLength(2, ErrorMessage = "Specifications must be atleast 100 char long.")]
    public string? Specifications { get; set; } // must be a html with bullet points
     [Range(1,int.MaxValue,ErrorMessage ="Category Id out of range.")]
    public int? CategoryId { get; set; }
    [Range(1,int.MaxValue,ErrorMessage ="Brand Id out of range.")]
    public int? BrandId { get; set; }
    public List<UpsertProductVariantDto> ? ProductVariantDto {get;set;}
     //[Required(ErrorMessage = "DiscountedPrice is a required field.")]
    // [Range(1, int.MaxValue, ErrorMessage = "DiscountedPrice cannot be negative or out of range.")]
    // public decimal DiscountedPrice { get; set; }
    // [Required(ErrorMessage = "ActualPrice is a required field.")]
    // [Range(1, int.MaxValue, ErrorMessage = "ActualPrice cannot be negative or out of range.")]
    // public decimal ActualPrice {get;set;}
    // [Required(ErrorMessage = "Please add atleast one image of product.")]
    // public List<string>? Images { get; set; }
}
