using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APILiteNET.API
{
    public class ErrorController : APIControllerBase
    {
        public ActionResult NormalError()
        {
            Response.StatusCode = 400;

            return this.APIResult(new
            {
                error = "Normal error.",
                url = Request.Url.AbsolutePath,
                method = Request.HttpMethod,
            });
        }

        public ActionResult APIError(Exception error)
        {
            var message = error.Message;

            //API EXCEPTION
            var aPIError = error as APIException;

            //HTTP EXCEPTION
            var hTTPError = error as HttpException;

            //OR OTHER EXCEPTION

            if (aPIError != null)
            {
                Response.StatusCode = aPIError.HTTPStatusCode;
            }
            else if (hTTPError != null)
            {
                Response.StatusCode = hTTPError.GetHttpCode();

                switch (hTTPError.GetHttpCode())
                {
                    case 404:
                        message = "Requested path can not be found";
                        break;
                    default:
                        message = "Server Fault";
                        break;
                }
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                message = error.Message;
            }

            try
            {
                return this.APIResult(new
                {
                    error = message,
                    url = Request.Url.AbsolutePath,
                    method = Request.HttpMethod,
#if DEBUG
                    type = error.GetType().FullName,
                    stack_trace = error.StackTrace,
#endif
                });
            }
            catch (APIException ex)
            {
                //LogHelper.Error(error);

                return new ContentResult
                {
                    ContentEncoding = APIControllerBase.ContentEncoding,
                    ContentType = "text/plain",
                    Content = ex.Message
                };
            }
        }
    }
}