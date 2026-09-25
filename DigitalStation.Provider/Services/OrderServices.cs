using System;
using System.Linq.Expressions;
using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Enums;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Provider.Database;
using Azure;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace DigitalStation.Provider.Services;

public class OrderServices : IOrderServices
{
    private readonly dbContext _dbContext;
    private readonly IEmailServices emailServices;
    public OrderServices(dbContext _dbContext, IEmailServices emailServices)
    {
        this._dbContext = _dbContext;
        this.emailServices = emailServices;
    }
    public async Task<bool> ChangeAddressAsync(int orderId, int addressId)
    {
        try
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new Exception(ResponseCode.InvalidOrderId);
            }
            var address = await _dbContext.Addresses.FindAsync(addressId);
            if (address == null)
            {
                throw new Exception(ResponseCode.InvalidAddressId);
            }
            order.AddressId = address.Id;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            throw;
        }
    }
    public async Task<bool> ChangeStatusAsync(int Id, OrderStatus orderStatus)
    {
        try
        {
            if (orderStatus == OrderStatus.Delivered)
            {
                List<OrderItem> orderItems = await _dbContext.OrderItems.Where(x => x.OrderId == Id && x.Status == OrderStatus.Placed).ToListAsync();
                orderItems.ForEach(x => x.Status = orderStatus);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else if (orderStatus == OrderStatus.Cancelled)
            {
                var orderItem = await _dbContext.OrderItems.Where(x => x.Id == Id && x.Status == OrderStatus.Placed).FirstOrDefaultAsync();
                if (orderItem == null)
                {
                    throw new Exception(ResponseCode.InvalidOrderItemId);
                }
                if (orderItem.Order.PaymentMode == PaymentMode.Prepaid)
                {
                    throw new Exception(ResponseCode.PrepaidOrderCancelError);
                }
                orderItem.Status = orderStatus;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else return false;
        }
        catch
        {
            throw;
        }
    }
    public async Task<int> AddAsync(AddOrderDto dto)
    {
        try
        {
            if (dto.Items.Count == 0)
            {
                throw new Exception(ResponseCode.OrderItemsEmpty);
            }
            if (dto.PaymentMode == PaymentMode.Prepaid && (dto.RazorPayId == null || dto.RazorPayId <= 0))
            {
                throw new Exception(ResponseCode.RazorPayIdRequired);
            }
            var user = await _dbContext.Users.FindAsync(dto.UserId);
            if (user == null)
            {
                throw new Exception(ResponseCode.InvalidUserId);
            }
            var address = await _dbContext.Addresses.FindAsync(dto.AddressId);
            if (address == null)
            {
                throw new Exception(ResponseCode.InvalidAddressId);
            }
            if (dto.PaymentMode == PaymentMode.Prepaid)
            {
                var razorPayDetail = await _dbContext.RazorPayDetails.FindAsync(dto.RazorPayId);
                if (razorPayDetail == null)
                {
                    throw new Exception(ResponseCode.InvalidRazorPayId);
                }
            }
            foreach (var item in dto.Items)
            {
                var productVariant = await _dbContext.ProductVariants.FindAsync(item.ProductVariantId);
                if (productVariant == null)
                {
                    throw new Exception(ResponseCode.InvalidProductVariantId);
                }
            }
            var order = Order.Mapper(dto);
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            var orderItems = new List<OrderItem>();
            foreach (var item in dto.Items)
            {
                var orderItem = OrderItem.Mapper(item);
                orderItem.OrderId = order.Id;
                orderItems.Add(orderItem);
            }
            await _dbContext.OrderItems.AddRangeAsync(orderItems);
            await _dbContext.SaveChangesAsync();
            await emailServices.SendEmail(user.Gmail, EmailType.OrderConfirmation, $"{order.Id}");
            return order.Id;
        }
        catch
        {
            throw;
        }
    }
}
