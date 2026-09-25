using System.Runtime.CompilerServices;
using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Contract.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace DigitalStation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : BaseController
    {
        private readonly IBrandServices brandServices;
        public BrandController(IBrandServices brandServices)
        {
            this.brandServices = brandServices;
        }
        [HttpGet("{categoryId}")]
        public async Task<ActionResult<ApiResponse<List<IdAndNameDto>>>> Get(int categoryId)
        {
            try
            {
                if(categoryId <= 0)
                {
                    return BadRequest(new ApiResponse<string>(false,ResponseCode.InvalidCategoryId));
                }
                var brands = await brandServices.GetAsync(categoryId);
                return Ok(new ApiResponse<List<IdAndNameDto>>(true,ResponseCode.FetchedSuccessfully,brands));
            }
            catch (Exception ex)
            {
                // if(ex.Message == ResponseCode.NoBrandsExists)
                // {
                //     return NotFound(new ApiResponse<string>(false,ex.Message));
                // }
                return HandleException(ex);
            }
        }
        
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Add(AddBrandDto dto)
        {
            try
            {
                if(dto == null)
                {
                    return BadRequest(new ApiResponse<string>(false,ResponseCode.ObjectNullException));
                }
                int brandId = await brandServices.AddAsync(dto);
                return Ok(new ApiResponse<int>(true,ResponseCode.AddedSuccessfully,brandId));
            }
            catch(Exception ex)
            {
                // if(ex.Message == ResponseCode.BrandAlreadyExists)
                // {
                //     return Conflict(new ApiResponse<string>(false,ex.Message));
                // }
                // if(ex.Message == ResponseCode.InvalidCategoryId)
                // {
                //     return NotFound(new ApiResponse<string>(false,ex.Message));
                // }
                // throw;
                return HandleException(ex);
            }
        }
    }
}
