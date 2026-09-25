using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalStation.Contract.Models;

public class Cart : Base
{
    public int Id {get;set;}
    public int Quantity {get;set;}
    public int UserId {get;set;}
    [ForeignKey(nameof(UserId))]
    public User User {get;set;} = null!;
    public int ProductVariantId {get;set;}
    [ForeignKey(nameof(ProductVariantId))]
    public ProductVariant ProductVariant {get;set;} = null!;
}
