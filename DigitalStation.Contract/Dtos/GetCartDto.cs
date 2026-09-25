using System;

namespace DigitalStation.Contract.Dtos;

public class GetCartDto : GetWishlistDto
{
    public int Quantity {get;set;}
}
