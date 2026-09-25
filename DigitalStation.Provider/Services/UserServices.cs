
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Dtos.UserDtos;
using DigitalStation.Contract.Enums;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Provider.Database;
using Azure;
using Microsoft.EntityFrameworkCore;

namespace DigitalStation.Provider.Services
{
    public class UserServices : IUserServices
    {
        private readonly dbContext _dbContext;
        private readonly IJwtServices jwtServices;
        public UserServices(dbContext _dbContext, IJwtServices jwtServices)
        {
            this._dbContext = _dbContext;
            this.jwtServices = jwtServices;
        }
        public async Task<bool> CheckUserExists(string gmail)
        {
            try
            {
                return await _dbContext.Users.AnyAsync(x => x.Gmail == gmail);
            }
            catch
            {
                throw;
            }
        }
        public async Task<int> AddUser(AddUserDto dto)
        {
            try
            {
                var user = await CheckUserExists(dto.Gmail);
                if (user)
                {
                    throw new Exception(ResponseCode.UsernameAlreadyExists);
                }
                var newUser = User.Mapper(dto);
                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();
                await AddLogin(newUser.Id, dto.Gmail, dto.Password);
                return newUser.Id;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                if (id == 0)
                {
                    throw new Exception(ResponseCode.InvalidUserId);
                }
                var user = await _dbContext.Users.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
                if (user == null)
                {
                    throw new Exception(ResponseCode.InvalidUserId);
                }
                user.IsActive = false;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<GetUserDto> GetUser(int id)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    throw new Exception(ResponseCode.InvalidUserId);
                }
                var userDto = GetUserDto.Mapper(user);
                return userDto;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Gmail == dto.Username);
                if (user == null)
                {
                    throw new Exception(ResponseCode.InvalidUserId);
                }
                var login = await _dbContext.Logins.FirstOrDefaultAsync(x => x.UserId == user.Id);
                if (login == null)
                {
                    throw new Exception(ResponseCode.LoginnotExists);
                }
                if (!String.IsNullOrEmpty(dto.OldPassword))
                {
                    if (login.Password != dto.OldPassword)
                        throw new Exception(ResponseCode.OldPasswordMismatched);
                }
                if (login.Password == dto.NewPassword)
                    throw new Exception(ResponseCode.SamePasswordIssue);

                login.Password = dto.NewPassword;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        #region  helper
        public async Task<int> AddLogin(int userId, string username, string password)
        {
            try
            {
                var newLogin = new Login
                {
                    Username = username,
                    Password = password,
                    UserId = userId
                };
                await _dbContext.Logins.AddAsync(newLogin);
                await _dbContext.SaveChangesAsync();
                return newLogin.Id;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
