using System;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Enums;

namespace DigitalStation.Contract.Models;

public class OrderItem : Base
{
    public int Id { get; set; }
    public Guid OrderItemGuid {get;set;} = Guid.NewGuid();
    public int OrderId { get; set; }
    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = null!;
    public int ProductVariantId { get; set; }
    [ForeignKey(nameof(ProductVariantId))]
    public ProductVariant ProductVariant { get; set; } = null!;
    public OrderStatus Status { get; set; } 
    public int Quantity { get; set; }
    public static OrderItem Mapper(AddOrderItemDto dto)
    {
        return new OrderItem
        {
            ProductVariantId = dto.ProductVariantId,
            Quantity = dto.Quantity
        };
    }
}
