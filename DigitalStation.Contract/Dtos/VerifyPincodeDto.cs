using System;

namespace DigitalStation.Contract.Dtos;

public class VerifyPincodeDto
{
    public int PincodeId {get;set;}
    public string CityName {get;set;} = null!;
    public string StateName {get;set;} = null!;
}
