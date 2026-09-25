using DigitalStation.Contract.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalStation.Contract.Models
{
    public class Region : Base
    {
        public int Id {get;set;}
        public string Name { get; set; } = null!; 
        public RegionType RegionType { get; set; }
        public int ? ParentId { get; set; }

        [ForeignKey(nameof(ParentId))]
        public Region Parent { get; set; } = null!; 
    }
}
