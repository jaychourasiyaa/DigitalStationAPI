using System;

namespace DigitalStation.Contract.Dtos;

public class GetAddressDto
{
    public int Id {get;set;}
    public string Name {get;set;} = null!;
    public string PhoneNumber {get;set;} = null!;
    public string? AlternatePhoneNumber {get;set;}
    public string? Locality {get;set;}
    public string CompleteAddress {get;set;} = null!;
    public string? Landmark {get;set;}
    public string City {get;set;} = null!;
    public string State {get;set;} = null!;
    public int Pincode {get;set;}
}
