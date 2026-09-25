using System.Runtime.ExceptionServices;
using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Dtos.UserDtos;
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
    public class AuthorizationController : BaseController
    {
        private IAuthorizationServices authorizationService;
        public AuthorizationController(IAuthorizationServices authorizationServices)
        {
            this.authorizationService = authorizationServices;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login( LoginDto dto)
        {
            try
            {
                var loginResponse = await authorizationService.LoginUser(dto);
                return Ok(new ApiResponse<LoginResponseDto>(true,"Logged in Successfully",loginResponse));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
