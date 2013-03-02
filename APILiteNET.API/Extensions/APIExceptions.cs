using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APILiteNET.API
{
    public class APIException : Exception
    {
        public int HTTPStatusCode { get; private set; }

        public APIException(string message, System.Net.HttpStatusCode statusCode)
            : base(message)
        {
            this.HTTPStatusCode = (int)statusCode;
        }
    }

    public class APIMethodNotAllowedException : APIException
    {
        public APIMethodNotAllowedException()
            : base("HTTP method is not allowed", System.Net.HttpStatusCode.MethodNotAllowed)
        {
        }
    }

    public class APIParameterMissingException : APIException
    {
        public APIParameterMissingException(string paramName)
            : base("Parameter '" + paramName + "' is required and its value can not be empty or null", System.Net.HttpStatusCode.BadRequest)
        {
        }
    }

    public class APIParameterBadFormatException : APIException
    {
        public APIParameterBadFormatException(string paramName, string paramValue, string expected)
            : base("The value of '" + paramName + "' is '" + paramValue + "' but expected " + expected, System.Net.HttpStatusCode.BadRequest)
        {
        }
    }

    public class APIAuthorizeException : APIException
    {
        public APIAuthorizeException(string message)
            : base("Authorization failed: " + message, System.Net.HttpStatusCode.BadRequest)
        { }
    }
}