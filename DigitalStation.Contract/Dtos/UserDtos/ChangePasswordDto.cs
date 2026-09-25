using DigitalStation.Contract.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalStation.Contract.Dtos.UserDtos
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Username is a required field.")]
        public string Username {get;set;} = null!;
        [Required(ErrorMessage = "New Password is a required field.")]
        [MinLength(6,ErrorMessage ="Password must be 6 char long.")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$", ErrorMessage = "Password must be at least 6 characters long, contain at least one lowercase letter, one uppercase letter, one number, and one special character.")]
        public string NewPassword { get; set; } = null!;
        public string? OldPassword { get; set; }
    }
}
