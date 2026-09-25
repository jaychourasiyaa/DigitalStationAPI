using System.Reflection;
using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Contract.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace DigitalStation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductServices productServices;
        public ProductController(IProductServices productServices)
        {
            this.productServices = productServices;
        }
        [HttpPost("All")]
        public async Task<ActionResult<PaginationResponseDto<List<UpsertProductVariantDto>>>>GetAll(ProductQueryDto dto)
        {
            try
            {
                var products = await productServices.GetAllAsync(dto);
                if(products.Count == 0)
                {
                    return NotFound(new ApiResponse<string>(false,ResponseCode.ZeroEntriesFound));
                }
                return Ok(new ApiResponse<PaginationResponseDto<List<UpsertProductVariantDto>>>(true,ResponseCode.FetchedSuccessfully,products));
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }
        
        [HttpGet("byRootCategory/{Id}")]
        public async Task<ActionResult<List<UpsertProductVariantDto>>> GetRandomProduct(int Id)
        {
            try
            {
                var products = await productServices.GetRandomProductAsync(Id);
                if(products.Count == 0)
                {
                    return NotFound(new ApiResponse<string>(false,ResponseCode.ZeroEntriesFound));
                }
                return Ok(new ApiResponse<List<UpsertProductVariantDto>>(true,ResponseCode.FetchedSuccessfully,products));
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }
        
        [HttpGet("{productId:int}")]
        public async Task<ActionResult<ApiResponse<GetProductDetailsDto>>> Get(int productId)
        {
            try
            {
                if(productId <= 0)
                {
                    throw new Exception(ResponseCode.IdOutOfRange);
                }
                var result = await productServices.GetByIdAsync(productId);
                return Ok(new ApiResponse<GetProductDetailsDto>(true,ResponseCode.FetchedSuccessfully,result));
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }
        
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Upsert(UpsertProductDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new ApiResponse<string>(false, ResponseCode.ObjectNullException));
                }
                var productId = await productServices.UpsertAsync(dto);
                if (dto.Id != null && dto.Id != 0)
                {
                    return Ok(new ApiResponse<int>(true, ResponseCode.UpdatedSuccessfully, productId));
                }
                return Ok(new ApiResponse<int>(true, ResponseCode.AddedSuccessfully, productId));
            }
            catch (Exception ex)
            {
                // if (ex.Message == ResponseCode.InvalidBrandId || ex.Message == ResponseCode.InvalidCategoryId ||
                //    ex.Message == ResponseCode.InvalidProductId)
                // {
                //     return NotFound(new ApiResponse<string>(false,ex.Message));
                // }
                // return BadRequest(new ApiResponse<string>(false,ex.Message));
                return HandleException(ex);
            }
        }
        
        [Authorize]
        [HttpPost("variant/{productId:int}")]
        public async Task<ActionResult<ApiResponse<int>>> UpsertVariant(List<UpsertProductVariantDto> dto,int productId)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new ApiResponse<string>(false, ResponseCode.ObjectNullException));
                }
                var result = await productServices.UpsertProductVariantAsync(dto,productId);
                if (dto[0].Id != null && dto[0].Id != 0)
                {
                    return Ok(new ApiResponse<int>(true, ResponseCode.UpdatedSuccessfully, productId));
                }
                return Ok(new ApiResponse<bool>(true, ResponseCode.AddedSuccessfully, result));
            }
            catch (Exception ex)
            {
                // if (ex.Message == ResponseCode.InvalidBrandId || ex.Message == ResponseCode.InvalidCategoryId ||
                //    ex.Message == ResponseCode.InvalidProductId)
                // {
                //     return NotFound(new ApiResponse<string>(false,ex.Message));
                // }
                // return BadRequest(new ApiResponse<string>(false,ex.Message));
                return HandleException(ex);
            }
        }

        
        [HttpGet("productVariant{id:int}")]
        public async Task<ActionResult<ApiResponse<List<GetProductVariantDto>>>> GetDetailsByProductVarId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new Exception(ResponseCode.InvalidProductVariantId);
                }
                var response = await productServices.GetDetailsByProductVarIdAsync(id);
                return Ok(new ApiResponse<GetProductVariantDto>(true, ResponseCode.FetchedSuccessfully, response));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

    }
}
