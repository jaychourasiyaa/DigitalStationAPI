using System;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStation.Contract.Dtos;

namespace DigitalStation.Contract.Models;

public class Category : Base
{
    public int Id { get; set; }
    public Guid CategoryGuid {get;set;} = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? IconUrl { get; set; }
    public int? ParentId { get; set; }
    [ForeignKey(nameof(ParentId))]
    public Category Parent { get; set; } = null!;
    public static Category Mapper(AddCategoryDto dto)
    {
        return new Category{
            Name = dto.Name,
            IconUrl = dto.IconUrl,
            ParentId = dto.ParentCategoryId
        };
    }
}
