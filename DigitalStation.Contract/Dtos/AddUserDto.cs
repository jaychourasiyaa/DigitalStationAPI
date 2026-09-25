using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalStation.Contract.Dtos;

public class AddUserDto
{
    [Required(ErrorMessage ="Name is a required field.")]
    [MinLength(2,ErrorMessage ="Name must be atleast 2 char long.")]
    [MaxLength(100,ErrorMessage ="Name must be atmost 100 char long")]
    public string Name {get;set;} = null!;
    [Required(ErrorMessage ="Gmail is a required field.")]
    public string Gmail {get;set;} = null!;
    [Required(ErrorMessage ="Password is a required field.")]
    [MinLength(6,ErrorMessage ="Password must be atleast 6 char long")]
    public string Password {get;set;} = null!;
    
}
