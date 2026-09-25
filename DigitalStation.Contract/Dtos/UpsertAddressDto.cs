using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalStation.Contract.Dtos;

public class UpsertAddressDto
{
    [Range(1,int.MaxValue,ErrorMessage ="Id out of range.")]
    public int ? Id {get;set;}
    [Required(ErrorMessage ="UserId is a required field.")]
    [Range(1,int.MaxValue,ErrorMessage ="UserId out of range.")]
    public int UserId {get;set;}
    [Required(ErrorMessage ="Name is a required field.")]
    [MinLength(2,ErrorMessage ="Name must be atleast 2 char long.")]
    [MaxLength(50,ErrorMessage ="Name must be atmost 50 char long.")]
    public string Name {get;set;} = null!;
    [Required(ErrorMessage ="Phone number is a required field.")]
    [MinLength(10,ErrorMessage ="Phone number must be 10 digit.")]
    [MaxLength(10, ErrorMessage ="Phone number must be 10 digit.")]
    public string PhoneNumber {get;set;} = null!;
    [MinLength(10,ErrorMessage ="AlternatePhoneNumber number must be 10 digit.")]
    [MaxLength(10, ErrorMessage ="AlternatePhoneNumber number must be 10 digit.")]
    public string? AlternatePhoneNumber {get;set;}
    public string? Locality {get;set;}
    [Required(ErrorMessage ="CompleteAddress is a required field.")]
    public string CompleteAddress {get;set;} = null!;
    public string? Landmark {get;set;}
    [Required(ErrorMessage ="PincodeId is a required field.")]
    [Range(1,int.MaxValue,ErrorMessage ="PincodeId out of range.")]
    public int PincodeId {get;set;}
}
