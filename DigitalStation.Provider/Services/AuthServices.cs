using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Dtos.UserDtos;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Provider.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DigitalStation.Provider.Services;

public class AuthServices : IAuthorizationServices
{
    private readonly dbContext _dbContext;
    private readonly IConfiguration configuration;

    public AuthServices(dbContext _dbContext, IConfiguration configuration)
    {
        this._dbContext = _dbContext;
        this.configuration = configuration;
    }
    public string GenerateToken(int id, string name, string role, string gmail)
    {


        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var claims = new[]
        {

                 new Claim("Name", name),
                 new Claim("Id", id.ToString()),
                 new Claim(ClaimTypes.Role, role),
                 new Claim("Gmail",gmail)

             };


        var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: credential
            );

        return tokenHandler.WriteToken(token);
    }

    public async Task<LoginResponseDto> LoginUser(LoginDto dto)
    {
        try
        {   // trying to find user with given gmail and password
            var userDto = new GetUserDto();
            string token = "";
            var login = await _dbContext.Logins
                    .Where(e => e.Username == dto.Username && e.Password == dto.Password)
                    .FirstOrDefaultAsync();
            if (login == null)
            {
                throw new Exception(ResponseCode.InvalidUsernamePassword);
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Gmail == dto.Username);
            if (user != null)
            {
                userDto.Id = user.Id;
                userDto.ProfilePictureUrl = user.ProfilePictureUrl;
                userDto.Gmail = user.Gmail;
                userDto.UserGuid = user.UserGuid;
                userDto.UserRole = user.UserRole;
                userDto.Name = user.Name;
                token = GenerateToken(user.Id, user.Name, user.UserRole.ToString(), user.Gmail);
            }
            if (token == "")
            {
                throw new Exception("Unable to generate Token");
            }
            // sending token along with some details
            var loginResponseDto = new LoginResponseDto
            {
                User = userDto,
                Token = token,
            };
            return loginResponseDto;
        }
        catch 
        {
            throw;
        }
    }
}

