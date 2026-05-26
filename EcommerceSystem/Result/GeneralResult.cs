using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.Result
{
    public class GeneralResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }=string.Empty;
        public Dictionary<string, Errors>? Error { get; set; }

        public static GeneralResult IsSucces(string message) => new GeneralResult
        {
            Success = true,
            Message = message
        };
        public static GeneralResult Failure(string message, Dictionary<string, Errors> error) => new GeneralResult
        {
            Success = false,
            Message = message,
            Error = error
        };

        public static GeneralResult NotFound(string message)=> new GeneralResult()
        {
            Success=false,
            Message = message
        };
    }

    public class GeneralResult<T> : GeneralResult
    {
        public T? Data { get; set; }
        public static GeneralResult<T> IsSucces(string message, T data) => new GeneralResult<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
        public static new GeneralResult<T> Failure(string message, Dictionary<string, Errors> error) => new GeneralResult<T>
        {
            Success = false,
            Message = message,
            Error = error
        };

    }
}
