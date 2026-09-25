using DigitalStation.Contract.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalStation.Contract.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public GetUserDto ? User { get; set; }
    }
}
