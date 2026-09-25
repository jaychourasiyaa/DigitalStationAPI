using DigitalStation.Contract.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalStation.Contract.Interfaces
{
    public interface IJwtServices 
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Gmail {  get; set; }
        public UserRole Role { get; set; }
    }
}
