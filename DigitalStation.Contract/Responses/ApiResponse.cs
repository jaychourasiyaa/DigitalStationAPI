using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalStation.Contract.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T Data { get; set; } = default!;
        public ApiResponse()
        {
            Success = true;
            Message = null;
        }
        public ApiResponse( bool success, string? message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
        public ApiResponse(bool sucess,string message)
        {
            Success = sucess;
            Message = message;
        }
        // public ApiResponse(T data)
        // {
        //     Data = data;
        // }
    }
}
