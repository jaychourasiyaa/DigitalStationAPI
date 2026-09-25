using System.Linq.Expressions;
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
    public class CategoryController : BaseController
    {
        private readonly ICategoryServices categoryServices;
        public CategoryController(ICategoryServices categoryServices)
        {
            this.categoryServices = categoryServices;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Add(AddCategoryDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new ApiResponse<string>(false, ResponseCode.ObjectNullException));
                }
                var categoryId = await categoryServices.AddAsync(dto);
                return Ok(new ApiResponse<int>(true, ResponseCode.AddedSuccessfully, categoryId));
            }
            catch (Exception ex)
            {
                // if (ex.Message == ResponseCode.InvalidParentCategoryId)
                // {
                //     return NotFound(new ApiResponse<string>(false, ex.Message));
                // }
                // if (ex.Message == ResponseCode.CategoryAlreadyExists)
                // {
                //     return Conflict(new ApiResponse<string>(false, ex.Message));
                // }
                // throw;
                return HandleException(ex);
            }
        }
        [HttpGet("getRootCategories")]
        public async Task<ActionResult<ApiResponse<List<IdAndNameDto>>>> Get()
        {
            try
            {
                var categories = await categoryServices.GetRootCategoryAsync();
                return Ok(new ApiResponse<List<IdAndNameDto>>(true, ResponseCode.FetchedSuccessfully, categories));
            }
            catch(Exception ex)
            {
                // return BadRequest(new ApiResponse<string>(false,ex.Message));
                return HandleException(ex);
            }
        }
        [HttpGet("getSubCategory/{id}")]
        public async Task<ActionResult<ApiResponse<List<IdAndNameDto>>>> GetById(int id)
        {
            try
            {
                var categories = await categoryServices.GetSubCategoryAsync(id);
                return Ok(new ApiResponse<List<IdAndNameDto>>(true, ResponseCode.FetchedSuccessfully, categories));
            }
            catch(Exception ex)
            {
                // return BadRequest(new ApiResponse<string>(false,ex.Message));
                return HandleException(ex);
            }
        }
    }
}
