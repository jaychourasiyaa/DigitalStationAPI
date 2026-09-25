
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
    public interface IUserServices
    {
        public Task<int> AddUser(AddUserDto dto);
        public Task<GetUserDto> GetUser(int id);
        Task<bool> ChangePasswordAsync(ChangePasswordDto dto);

    }
}
