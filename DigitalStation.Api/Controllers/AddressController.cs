using System.Reflection.Metadata;
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
    public class AddressController : BaseController
    {
        private readonly IAddressServices addressServices;
        public AddressController(IAddressServices addressServices)
        {
            this.addressServices = addressServices;
        }
        [HttpGet("verify/{pincode:int}")]
        public async Task<ActionResult<ApiResponse<VerifyPincodeDto>>> Verify(int pincode)
        {
            try
            {
                if(pincode < 100000 || pincode > 999999)
                {
                    throw new Exception(ResponseCode.PincodeOutOfRange);
                }
                var result = await addressServices.VerifyPincodeAsync(pincode);
                if(result == null)
                {
                    throw new Exception(ResponseCode.NotServiceable);
                }
                return Ok(new ApiResponse<VerifyPincodeDto>(true,ResponseCode.FetchedSuccessfully,result));
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }
        
        [Authorize]
        [HttpGet("{userId:int}")]
        public async Task<ActionResult<ApiResponse<List<GetAddressDto>>>> Get(int userId)
        {
            try
            {
                if(userId <= 0)
                {
                    throw new Exception(ResponseCode.IdOutOfRange);
                }
                var addresses = await addressServices.GetAsync(userId);
                if(addresses.Count == 0)
                {
                    throw new Exception(ResponseCode.ZeroEntriesFound);
                }
                return Ok(new ApiResponse<List<GetAddressDto>>(true,ResponseCode.FetchedSuccessfully,addresses));
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Upsert(UpsertAddressDto dto)
        {
            try
            {
                var addressId = await addressServices.UpsertAddressAsync(dto);
                return Ok(new ApiResponse<int>(true,ResponseCode.AddedSuccessfully,addressId));
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                if(id <= 0)
                {
                    throw new Exception(ResponseCode.IdOutOfRange);
                }
                var result = await addressServices.DeleteAsync(id);
                return Ok(new ApiResponse<bool>(true,ResponseCode.DeletedSuccessfully,result));
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
