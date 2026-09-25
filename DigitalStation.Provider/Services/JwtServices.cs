using DigitalStation.Contract.Enums;
using DigitalStation.Contract.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace DigitalStation.Provider.Services
{
    public class JwtServices : IJwtServices
    {
        public string Name { get; set; } = null!;
        public int Id { get; set; }
        public string Gmail { get; set; } = null!;
        public UserRole Role { get; set; }
        public JwtServices(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user != null)
            {
                var idClaim = user.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                Id = idClaim != null ? Convert.ToInt32(idClaim) : 0;
                var NameClaim = user.Claims.FirstOrDefault(c => c.Type == "Name")?.Value;
                Name = NameClaim != null ? NameClaim : string.Empty;
                var GmailClaim = user.Claims.FirstOrDefault(c => c.Type == "Gmail")?.Value;
                Gmail = GmailClaim != null ? GmailClaim : string.Empty;
                var RoleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                // Extract and map 'Role' claim
                var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (Enum.TryParse(typeof(UserRole), roleClaim, true, out var role))
                {
                    Role = (UserRole)role;
                }

            }
        }
    }
}
