using System;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStation.Contract.Dtos;

namespace DigitalStation.Contract.Models;

public class Brand : Base
{
    public int Id { get; set; }
    public Guid BrandGuid { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? IconUrl { get; set; }
    public int? CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = null!;

    public static Brand Mapper(AddBrandDto dto)
    {
        return new Brand
        {
            Name = dto.Name.ToUpper(),
            CategoryId = dto.CategoryId,
            IconUrl = dto.IconUrl
        };
    }

}
