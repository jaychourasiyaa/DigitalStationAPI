using System;
using DigitalStation.Contract.Enums;
using DigitalStation.Contract.Models;

namespace DigitalStation.Contract.Dtos;

public class GetUserDto
{
    public int Id { get; set; }
    public string Gmail { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Guid UserGuid { get; set; } = new Guid();
    public UserRole UserRole { get; set; }
    public string? ProfilePictureUrl { get; set; } // default avatar needs to be set
    public static GetUserDto Mapper(User user)
    {
        return new GetUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Gmail = user.Gmail,
            ProfilePictureUrl = user.ProfilePictureUrl,
            UserGuid = user.UserGuid
        };
    }
}
