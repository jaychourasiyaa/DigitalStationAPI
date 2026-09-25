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
    public class OrderController : BaseController
    {
        private readonly IOrderServices orderServices;
        public OrderController(IOrderServices orderServices)
        {
            this.orderServices = orderServices;
        }
    [Authorize]    
    [HttpPost]
    public async Task<ActionResult<ApiResponse<int>>> Add(AddOrderDto dto)
    {
        try
        {
            if(dto == null)
            {
                return BadRequest(new ApiResponse<string>(false, ResponseCode.ObjectNullException)); 
            }
            var orderId = await orderServices.AddAsync(dto);
            return Ok(new ApiResponse<int>(true,ResponseCode.AddedSuccessfully,orderId));
        }
        catch(Exception ex)
        {
            return HandleException(ex);
        }
    }
    }
}
