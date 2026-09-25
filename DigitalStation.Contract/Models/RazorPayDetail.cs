using System;

namespace DigitalStation.Contract.Models;

public class RazorPayDetail : Base
{
    public int Id {get;set;}
    public string RazorpayOrderId { get; set; } = null!;      // Razorpay order ID (from their API)
    public string RazorpayPaymentId { get; set; } = null!;    // Razorpay payment transaction ID
    public string RazorpaySignature { get; set; } = null!;    // Razorpay payment signature for verification
    public string PaymentStatus { get; set; } = null!;        // e.g., "Created", "Authorized", "Captured", "Failed"
    public string PaymentMethod { get; set; } = null!;        // e.g., "card", "upi", "wallet"
}
