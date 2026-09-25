using System;
using System.ComponentModel.DataAnnotations;
using DigitalStation.Contract.Enums;

namespace DigitalStation.Contract.Dtos;

public class AddOrderDto
{
    [Required(ErrorMessage ="UserId is a required filed.")]
    [Range(1,int.MaxValue,ErrorMessage ="UserId out of Range")]
    public int UserId {get;set;}
    [Required(ErrorMessage ="AddressId is a required filed.")]
    [Range(1,int.MaxValue,ErrorMessage ="UserId out of Range")]
    public int AddressId {get;set;}
    [Required(ErrorMessage ="PaymentMode is a required filed.")]
    public PaymentMode PaymentMode {get;set;}
    [Range(1,int.MaxValue,ErrorMessage ="RazorPayId out of Range")]
    public int ? RazorPayId {get;set;}
    [Required]
    public List<AddOrderItemDto> Items {get;set;} = null!;
}
