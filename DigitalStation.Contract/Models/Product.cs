using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;
using DigitalStation.Contract.Dtos;

namespace DigitalStation.Contract.Models;

public class Product : Base
{
    public int Id { get; set; }
    public Guid ProductGuid { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Specifications { get; set; } // must be a html with bullet points
    public int? CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = null!;
    public int? BrandId { get; set; }
    [ForeignKey(nameof(BrandId))]
    public Brand Brand { get; set; } = null!;
    public static Product Mapper(UpsertProductDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Specifications = dto.Specifications,
            CategoryId = dto.CategoryId,
            BrandId = dto.BrandId
        };
    }
}
