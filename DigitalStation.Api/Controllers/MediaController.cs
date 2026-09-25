using System.Reflection.Metadata.Ecma335;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Responses;
using DigitalStation.Provider.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : BaseController
    {
        private readonly CloudinaryServices mediaServices;
        public MediaController(CloudinaryServices imageServices)
        {
            this.mediaServices = imageServices;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> Add(IFormFile formFile)
        {
            try
            {
                var imageUrl = await mediaServices.UploadMediaAsync(formFile);
                if(imageUrl == "")
                {
                    return BadRequest(new ApiResponse<string>(false,"Unable to upload media"));
                }
                return Ok(new ApiResponse<string>(true,"Media Uploaded Successfully.",imageUrl));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
