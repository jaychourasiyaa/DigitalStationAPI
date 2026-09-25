using System;
using System.ComponentModel.DataAnnotations;
using DigitalStation.Contract.Enums;

namespace DigitalStation.Contract.Dtos;

public class SendEmailDto
{
    [Required(ErrorMessage ="Gmail is a required filed.")]
    public string Gmail{get;set;} = null!; 
    
    [Required(ErrorMessage ="EmailType is a required filed.")]
    public EmailType? EmailType {get;set;}
    public string? Name {get;set;} = "User";
}
