
using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Dtos.UserDtos;
using DigitalStation.Contract.Enums;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Contract.Responses;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DigitalStation.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserServices userServices;
        private readonly IEmailServices emailServices;

        public UserController(IUserServices userServices, IEmailServices emailServices)
        {
            this.userServices = userServices;
            this.emailServices = emailServices;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> AddUser(AddUserDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new ApiResponse<string>(false, ResponseCode.ObjectNullException));
                }
                var userId = await userServices.AddUser(dto);
                return Ok(new ApiResponse<int>(true, ResponseCode.AddedSuccessfully, userId));
            }
            catch (Exception ex)
            {
                if (ex.Message == ResponseCode.UsernameAlreadyExists)
                {
                    return Conflict(new ApiResponse<string>(false, ex.Message));
                }
                throw;
            }
        }
        
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetUserDto>>> GetUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<string>(false,ResponseCode.InvalidUserId));
                }
                var user = await userServices.GetUser(id);
                return Ok(new ApiResponse<GetUserDto>(true,ResponseCode.AddedSuccessfully,user));
            }
            catch (Exception ex)
            {
                if(ex.Message == ResponseCode.InvalidUserId)
                {
                    return NotFound(new ApiResponse<string>(false,ex.Message));
                }
                throw;
            }
        }
        
        [Authorize]
        [HttpPut("changePassword")]
        public async Task<ActionResult<ApiResponse<bool>>> ChangePassword(ChangePasswordDto dto)
        {
            try
            {
                if(dto == null)
                {
                    throw new Exception(ResponseCode.ObjectNullException);
                }
                await userServices.ChangePasswordAsync(dto);
                return Ok(new ApiResponse<bool>(true,ResponseCode.UpdatedSuccessfully,true));
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("sendOtp")]
        public async Task<ActionResult<ApiResponse<bool>>> Send(SendEmailDto dto)
        {
            if(dto == null)
            {
                throw new Exception(ResponseCode.ObjectNullException);
            }
            try
            {
                var result = await emailServices.SendEmail(dto.Gmail, dto.EmailType, dto.Name);
                if(result)
                {
                    return Ok(new ApiResponse<bool>(true,ResponseCode.OtpSentSuccessfully,true));
                }
                return BadRequest(new ApiResponse<string>(false,"Unable to send otp."));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("verify/{gmail}/{otp}")]
        public async Task<ActionResult<ApiResponse<string>>> Verify(string gmail, string otp)
        
        {
            try
            {
                string ? result = await emailServices.VerifyOtp(gmail, otp);
                if (result == null)
                {
                    return NotFound(new ApiResponse<string>(false,ResponseCode.InvalidOTP));
                }
                return Ok(new ApiResponse<string>(true,ResponseCode.OTPVerified,result));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
