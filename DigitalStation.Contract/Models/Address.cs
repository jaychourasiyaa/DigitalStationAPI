using System;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStation.Contract.Dtos;

namespace DigitalStation.Contract.Models;

public class Address
{
    public int Id {get;set;}
    public int UserId {get;set;}
    public string Name {get;set;} = null!;
    public string PhoneNumber {get;set;} = null!;
    public string? AlternatePhoneNumber {get;set;}
    public string? Locality {get;set;}
    public string CompleteAddress {get;set;} = null!;
    public string? Landmark {get;set;}
    public int PincodeId {get;set;}
    [ForeignKey(nameof(PincodeId))]
    public Pincode Pincode {get;set;} = null!;
    [ForeignKey(nameof(UserId))]
    public User User {get;set;} = null!;
    public static Address Mapper(Address address,UpsertAddressDto dto)
    {
            address.UserId = dto.UserId;
            address.Name = dto.Name;
            address.PhoneNumber = dto.PhoneNumber;
            address.AlternatePhoneNumber = dto.AlternatePhoneNumber;
            address.Locality = dto.Locality;
            address.CompleteAddress = dto.CompleteAddress;
            address.Landmark = dto.Landmark;
            address.PincodeId = dto.PincodeId;
            return address;
    }

}
