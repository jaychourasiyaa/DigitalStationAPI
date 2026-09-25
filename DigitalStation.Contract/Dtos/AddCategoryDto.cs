using System;
using System.ComponentModel.DataAnnotations;
using DigitalStation.Contract.Models;

namespace DigitalStation.Contract.Dtos;

public class AddCategoryDto
{
    [Required(ErrorMessage ="Name is a required field.")]
    [MinLength(2, ErrorMessage ="Name must be atleast 2 char long.")]
    [MaxLength(100, ErrorMessage ="Name must be atmost 100 char long.")]
    public string Name {get;set;} = null!;
    [Range(1,int.MaxValue,ErrorMessage ="Parent Category Id out of range.")]
    [Required(ErrorMessage ="ParentCategoryId is a required field.")]
    public int ParentCategoryId {get;set;}
    public string ? IconUrl{get;set;}
}
