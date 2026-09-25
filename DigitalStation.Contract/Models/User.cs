using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Enums;
using DigitalStation.Contract.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalStation.Contract.Models
{
    public class User : Base
    {
        public int Id { get; set; }
        public string Gmail { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsLoggedIn { get; set; } = true;
        public Guid UserGuid { get; set; } = Guid.NewGuid();
        public UserRole UserRole {get;set;} = UserRole.Customer;
        public string? ProfilePictureUrl { get; set; } // default avatar needs to be set
        public static User Mapper(AddUserDto dto)
        {
            return new User{
                Gmail = dto.Gmail,
                Name = dto.Name
            };
        }
    }
}
