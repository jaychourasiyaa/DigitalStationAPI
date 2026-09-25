using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Enums;

namespace DigitalStation.Contract.Models;

public class Order : Base
{
    public int Id { get; set; }
    public Guid OrderGuid { get; set; } = Guid.NewGuid();
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    public int AddressId { get; set; }
    [ForeignKey(nameof(AddressId))]
    public Address Address { get; set; } = null!;
    //public decimal TotalAmount { get; set; }    // Total order amount (INR) of all orderItem        
    public PaymentMode PaymentMode { get; set; }
    public int? RazorpayId { get; set; }
    [ForeignKey(nameof(RazorpayId))]
    public RazorPayDetail RazorPayDetail { get; set; } = null!;
    public static Order Mapper(AddOrderDto dto)
    {
        return new Order
        {
            UserId = dto.UserId,
            AddressId = dto.AddressId,
            PaymentMode = dto.PaymentMode,
            RazorpayId = dto.RazorPayId != null ? dto.RazorPayId : null,
        };
    }
}
