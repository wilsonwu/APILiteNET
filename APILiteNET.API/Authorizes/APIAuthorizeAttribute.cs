using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using APILiteNET.Service;

namespace APILiteNET.API
{
    /// <summary>
    /// OAuth like authorization for api requests
    /// </summary>
    public class APIAuthorizeAttribute : ActionFilterAttribute
    {
        private static readonly string AuthSecretKey = "secretkey";
        private static readonly string AuthFormat = "format";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ValidateRequest(filterContext.HttpContext.Request);
            base.OnActionExecuting(filterContext);
        }

        private static void ValidateRequest(HttpRequestBase request)
        {
            string method = request.HttpMethod.ToUpper();
            string url = request.Url.AbsoluteUri;
            if (!string.IsNullOrEmpty(request.Url.Query))
            {
                url = url.Replace(request.Url.Query, string.Empty);
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            foreach (string k in request.Form.AllKeys)
            {
                parameters.Add(k, request.Form.Get(k));
            }
            foreach (string k in request.QueryString.AllKeys)
            {
                parameters.Add(k, request.QueryString.Get(k));
            }

            var tempParameters = from p in parameters where (p.Key != AuthSecretKey) && (p.Key != AuthFormat) orderby p.Key select p;
            Dictionary<string, string> lastParameters = tempParameters.ToDictionary<KeyValuePair<string, string>, string, string>(p => p.Key, p => p.Value);

            string secretKey = request.GetParameter(AuthSecretKey);
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new APIAuthorizeException("SecretKey is not found");
            }
            string correctSecretKey = PublicFunctions.URLEncode(url);
            foreach (string parameter in lastParameters.Keys)
            {
                correctSecretKey = correctSecretKey + parameters[parameter];
            }
            if (secretKey != correctSecretKey)
            {
                throw new APIAuthorizeException("SecretKey is invalid");
            }
        }
    }
}