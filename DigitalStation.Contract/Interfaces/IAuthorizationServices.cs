using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Dtos.UserDtos;
using DigitalStation.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalStation.Contract.Interfaces
{
    public interface IAuthorizationServices
    {
        public Task<LoginResponseDto> LoginUser(LoginDto dto);
    }
}
