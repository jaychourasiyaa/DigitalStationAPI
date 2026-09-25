using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalStation.Contract.Models;

public class Pincode : Base
{
    public int Id {get;set;}
    public int Value {get;set;}
    public int RegionId {get;set;}
    [ForeignKey(nameof(RegionId))]
    public Region City {get;set;} = null!;
}
