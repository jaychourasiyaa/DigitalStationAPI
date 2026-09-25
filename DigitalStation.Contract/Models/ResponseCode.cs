using System;
using Microsoft.AspNetCore.Http;


namespace DigitalStation.Contract.Models;

public static class ResponseCode
{
    public const string QuantityNegativeError = "Quantity must be greater than zero.";
    public const string InvalidCartId = "Cart Id is invalid.";
    public const string InvalidWishlistId = "Wishlist Id is invalid.";
    public const string ObjectNullException = "Object cannot be null.";
    public const string BrandAlreadyExists = "Brandname already exists.";
    public const string InvalidCategoryId = "Category Id is invalid.";
    public const string InvalidUserId = "User Id is invalid.";
    public const string AddedSuccessfully = "Added Successfully.";
    public const string PriceConfliction = "Actual Price cannot be less than Discounted Price.";
    public const string NoBrandsExists = "No Brand Exists under given category.";
    public const string InvalidBrandId = "Brand Id is invalid.";
    public const string InvalidParentCategoryId = "Parent Category Id is invalid.";
    public const string CategoryAlreadyExists = "Category with given name already exists.";
    public const string UpdatedSuccessfully = "Updated Successfully.";
    public const string DeletedSuccessfully = "Deleted Successfully.";
    public const string FetchedSuccessfully = "Fetched Successfully.";
    public const string UsernameAlreadyExists = "Username Already Exists.";
    public const string InvalidProductId = "Product Id is invalid.";
    public const string InvalidProductVariantId = "Product Variant Id is invalid.";
    public const string LoginnotExists = "Login not exist for given user.";
    public const string OldPasswordMismatched = "Old Password does not match.";
    public const string SamePasswordIssue = "Old and New Password must be different.";
    public const string InvalidUsernamePassword = "Username or password is invalid.";
    public const string InvalidPincodeId = "PincodeId is invalid.";
    public const string InvalidAddressId = "AddressId is invalid.";
    public const string IdOutOfRange = "Id out of range.";
    public const string ZeroEntriesFound = "No entries found.";
    public const string OtpSentSuccessfully = "OTP sent successfully.";
    public const string AccountNotExists = "Account not exist with given email.";
    public const string InvalidOTP = "OTP is invalid.";
    public const string OTPVerified = "OTP verified successfully.";
    public const string PincodeOutOfRange = "Pincode out of range.";
    public const string OrderItemsEmpty = "Please add atleast one item to place order.";
    public const string InvalidRazorPayId = "Invalid RazorPay Id.";
    public const string InvalidOrderItemId = "Invalid Order Item Id.";
    public const string InvalidOrderId = "Invalid Order Id.";
    public const string PrepaidOrderCancelError = "Please visit to nearest branch to cancel Prepaid Order.";
    public const string RazorPayIdRequired = "If Payment Mode is Prepaid RazorPayId is required.";
    public const string NotServiceable = "Service is not availabe in given Pincode, order using Amazon or flipkart Links.";
    
    public static readonly Dictionary<string, int> ErrorStatusCodes = new()
    {
        // Invalid or not found errors -> 404
        { ResponseCode.InvalidBrandId, StatusCodes.Status404NotFound },
        { ResponseCode.InvalidCategoryId, StatusCodes.Status404NotFound },
        { ResponseCode.InvalidProductId, StatusCodes.Status404NotFound },
        { ResponseCode.InvalidUserId, StatusCodes.Status404NotFound },
        { ResponseCode.InvalidParentCategoryId, StatusCodes.Status404NotFound },
        { ResponseCode.NoBrandsExists, StatusCodes.Status404NotFound },
        { ResponseCode.InvalidProductVariantId, StatusCodes.Status404NotFound },
        { ResponseCode.LoginnotExists, StatusCodes.Status404NotFound },
        { ResponseCode.InvalidPincodeId, StatusCodes.Status404NotFound },
        { ResponseCode.InvalidAddressId, StatusCodes.Status404NotFound },
        { ResponseCode.AccountNotExists, StatusCodes.Status404NotFound },
        { ResponseCode.InvalidOTP, StatusCodes.Status404NotFound },
        { ResponseCode.PincodeOutOfRange, StatusCodes.Status404NotFound },
        { ResponseCode.NotServiceable, StatusCodes.Status404NotFound },
        { ResponseCode.ZeroEntriesFound, StatusCodes.Status404NotFound },
        { ResponseCode.InvalidRazorPayId, StatusCodes.Status404NotFound },
        { ResponseCode.InvalidOrderItemId, StatusCodes.Status404NotFound },
        { ResponseCode.InvalidOrderId, StatusCodes.Status404NotFound },
        { ResponseCode.QuantityNegativeError, StatusCodes.Status404NotFound },
        { ResponseCode.InvalidCartId, StatusCodes.Status404NotFound },
        { ResponseCode.InvalidWishlistId, StatusCodes.Status404NotFound },

        // Already exists errors -> 409 Conflict
        { ResponseCode.BrandAlreadyExists, StatusCodes.Status409Conflict },
        { ResponseCode.CategoryAlreadyExists, StatusCodes.Status409Conflict },
        { ResponseCode.UsernameAlreadyExists, StatusCodes.Status409Conflict },
        { ResponseCode.PriceConfliction, StatusCodes.Status409Conflict },
        { ResponseCode.OldPasswordMismatched, StatusCodes.Status409Conflict },
        { ResponseCode.SamePasswordIssue, StatusCodes.Status409Conflict },
        { ResponseCode.InvalidUsernamePassword, StatusCodes.Status409Conflict },
        { ResponseCode.PrepaidOrderCancelError, StatusCodes.Status409Conflict },

        // Null object -> 400 Bad Request
        { ResponseCode.ObjectNullException, StatusCodes.Status400BadRequest },
        { ResponseCode.IdOutOfRange, StatusCodes.Status400BadRequest },
        { ResponseCode.OrderItemsEmpty, StatusCodes.Status400BadRequest },

        // Success messages (usually no error but just for reference if needed)
        // You typically don't return these in error handling
        { ResponseCode.AddedSuccessfully, StatusCodes.Status200OK },
        { ResponseCode.UpdatedSuccessfully, StatusCodes.Status200OK },
        { ResponseCode.DeletedSuccessfully, StatusCodes.Status200OK },
        { ResponseCode.FetchedSuccessfully, StatusCodes.Status200OK },
        { ResponseCode.OtpSentSuccessfully, StatusCodes.Status200OK },
        { ResponseCode.OTPVerified, StatusCodes.Status200OK },
    };
}



