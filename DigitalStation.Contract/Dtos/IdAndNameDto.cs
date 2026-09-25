using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalStation.Contract.Dtos
{
    public class IdAndNameDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ? IconUrl {get;set;}
    }
}
