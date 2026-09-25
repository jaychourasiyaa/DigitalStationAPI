using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Contract.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.Sig;

namespace DigitalStation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : BaseController
    {
        private readonly ICartServices cartServices;
        public CartController(ICartServices cartServices)
        {
            this.cartServices = cartServices;
        }
        [Authorize]
        [HttpPost()]
        public async Task<ActionResult<ApiResponse<int>>> Add(AddCartDto dto)
        {
            try
            {
                if (dto.ProductVariantId <= 0)
                {
                    throw new Exception(ResponseCode.InvalidProductVariantId);
                }
                if (dto.Quantity <= 0)
                {
                    throw new Exception(ResponseCode.QuantityNegativeError);
                }
                var cartId = await cartServices.UpsertAsync(dto);
                return Ok(new ApiResponse<int>(true, ResponseCode.AddedSuccessfully, cartId));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<GetCartDto>>>> Get()
        {
            try
            {
                var response = await cartServices.GetAsync();
                return Ok(new ApiResponse<List<GetCartDto>>(true, ResponseCode.FetchedSuccessfully, response));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<List<GetCartDto>>>> Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new Exception(ResponseCode.InvalidCartId);
                }
                var response = await cartServices.GetByIdAsync(id);
                return Ok(new ApiResponse<GetCartDto>(true, ResponseCode.FetchedSuccessfully, response));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [Authorize]
        [HttpDelete("{cartId}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int cartId)
        {
            try
            {
                if (cartId <= 0)
                {
                    throw new Exception(ResponseCode.InvalidCartId);
                }
                var response = await cartServices.DeleteAsync(cartId);
                return Ok(new ApiResponse<bool>(true, ResponseCode.DeletedSuccessfully, true));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
