using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalStation.Contract.Dtos;

public class AddOrderItemDto
{
    [Required(ErrorMessage ="ProductVariantId is a required filed.")]
    [Range(1,int.MaxValue,ErrorMessage ="ProductVariantId out of Range")]
    public int ProductVariantId {get;set;}
    [Required(ErrorMessage ="Quantity is a required filed.")]
    [Range(1,int.MaxValue,ErrorMessage ="Quantity out of Range")]
    public int Quantity {get;set;}
}
