using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Contract.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : BaseController
    {
        private readonly IWishlistServices wishlistServices;
        public WishlistController(IWishlistServices wishlistServices)
        {
            this.wishlistServices = wishlistServices;
        }
        [Authorize]
        [HttpPost("{productVariantId:int}")]
        public async Task<ActionResult<ApiResponse<int>>> Add(int productVariantId)
        {
            try
            {
                var wishlistId = await wishlistServices.AddAsync(productVariantId);
                return Ok(new ApiResponse<int>(true,ResponseCode.AddedSuccessfully,wishlistId));
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<GetWishlistDto>>>> Get()
        {
            try
            {
                var response = await wishlistServices.GetAsync();
                return Ok(new ApiResponse<List<GetWishlistDto>>(true,ResponseCode.FetchedSuccessfully,response));
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }
        [Authorize]
        [HttpDelete("{wishlistId}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int wishlistId)
        {
            try
            {
                if (wishlistId <= 0)
                {
                    throw new Exception(ResponseCode.InvalidWishlistId);
                }
                var response = await wishlistServices.DeleteAsync(wishlistId);
                return Ok(new ApiResponse<bool>(true,ResponseCode.DeletedSuccessfully,true));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
