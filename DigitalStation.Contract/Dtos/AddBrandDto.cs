using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalStation.Contract.Dtos;

public class AddBrandDto
{
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    [Required(ErrorMessage = "Name is a required field.")]
    public string Name { get; set; } = null!;
    [Range(1, int.MaxValue, ErrorMessage = "CategoryId out of range.")]
    [Required(ErrorMessage = "CategoryId is a required field.")]
    public int CategoryId { get; set; }
    public string? IconUrl { get; set; }
}
