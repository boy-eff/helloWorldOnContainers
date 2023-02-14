using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Domain.Enums;

namespace AuthService.Domain.Entities
{
    public class ServiceError
    {
        public ServiceError(ServiceErrorStatusCode statusCode, string message = "")
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ServiceErrorStatusCode StatusCode { get; init; }
        public string Message { get; init; }
    }
}