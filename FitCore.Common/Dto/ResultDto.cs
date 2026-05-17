using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Common.Dto
{

    public class ResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public static ResultDto Success(string message = null)
        {
            return new ResultDto
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static ResultDto Failure(string message)
        {
            return new ResultDto
            {
                IsSuccess = false,
                Message = message
            };
        }
    }
    public class ResultDto<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ResultDto<T> Success(T data, string message = null)
        {
            return new ResultDto<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message
            };
        }

        public static ResultDto<T> Failure(string message)
        {
            return new ResultDto<T>
            {
                IsSuccess = false,
                Message = message
            };
        }
    }

}
