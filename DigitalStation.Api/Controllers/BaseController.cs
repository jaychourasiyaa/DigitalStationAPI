using System.Security;
using DigitalStation.Contract.Models;
using DigitalStation.Contract.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected ActionResult HandleException(Exception ex)

        {
            int statusCode = StatusCodes.Status500InternalServerError;
            string message = ex.Message;

            if (ex.InnerException != null)
            {
                message += $" | Inner Exception: {ex.InnerException.Message}";
            }
            // Try from ResponseCode dictionary
            if (ResponseCode.ErrorStatusCodes.TryGetValue(ex.Message, out int mappedCode))
            {
                statusCode = mappedCode;

                // For 204 No Content, return empty response
                if (statusCode == StatusCodes.Status204NoContent)
                {
                    return StatusCode(statusCode);
                }
            }
            else
            {
                // Type-based fallback
                if (ex is ArgumentNullException or ArgumentException or FormatException)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                }
                else if (ex is UnauthorizedAccessException)
                {
                    statusCode = StatusCodes.Status401Unauthorized;
                }
                else if (ex is SecurityException)
                {
                    statusCode = StatusCodes.Status403Forbidden;
                }
                else if (ex is InvalidOperationException)
                {
                    statusCode = StatusCodes.Status409Conflict;
                }
                else if (ex is NotImplementedException)
                {
                    statusCode = StatusCodes.Status501NotImplemented;
                }
            }

            return StatusCode(statusCode, new ApiResponse<string>(false, message));
        }
    }
}
