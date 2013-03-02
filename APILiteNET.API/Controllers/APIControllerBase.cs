using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace APILiteNET.API
{
    public class APIControllerBase : Controller
    {
        public static readonly string JSONFormat = "json";
        public static readonly string XMLFormat = "xml";
        public static readonly string JSONPFormat = "jsonp";
        public static readonly string JSONContentType = "application/json";
        public static readonly string JavaScriptContentType = "application/x-javascript";
        public static readonly string XMLContentType = "application/xml";
        public static readonly Encoding ContentEncoding = Encoding.UTF8;

        private HttpMethod currentHttpMethod;

        public HttpMethod CurrentHttpMethod
        {
            get { return currentHttpMethod; }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Response.AddHeader("Access-Control-Allow-Origin", "*");

            currentHttpMethod = new HttpMethod(filterContext.HttpContext.Request.HttpMethod);
            base.OnActionExecuting(filterContext);
        }

        public ActionResult APIResult(object data)
        {
            if (data == null)
            {
                data = new { };
            }

            var format = Request.GetParameter("format");
            if (string.IsNullOrEmpty(format))
            {
                format = JSONFormat;
            }

            var callback = Request.GetParameter("callback");
            if (!string.IsNullOrEmpty(callback))
            {
                format = JSONPFormat;
            }

            if (JSONPFormat == format.ToLower() && string.IsNullOrEmpty(callback))
            {
                callback = string.Format("callback_{0:0}", (DateTime.UtcNow.ToUnixSeconds() * 1000));
            }

            switch (format.ToLower())
            {
                case "json":
                    return this.JsonResultFromData(data);
                case "xml":
                    return this.XmlResultFromData(data);
                case "jsonp":
                    return this.JsonpResultFromData(data, callback);
                default:
                    throw new APIException("The format '" + format + "' is not supported, use 'json'(Default), 'xml' or 'jsonp'('callback' required) instead", HttpStatusCode.BadRequest);
            }
        }

        protected virtual ActionResult XmlResultFromData(object data)
        {
            return new ContentResult
            {
                ContentType = APIControllerBase.XMLContentType,
                ContentEncoding = APIControllerBase.ContentEncoding,
                Content = data.ToXmlString()
            };
        }

        protected virtual ActionResult JsonResultFromData(object data)
        {
            return new ContentResult
            {
                ContentType = APIControllerBase.JSONContentType,
                ContentEncoding = APIControllerBase.ContentEncoding,
                Content = JsonConvert.SerializeObject(data, Formatting.Indented)
            };
        }

        protected virtual ActionResult JsonpResultFromData(object data, string callback)
        {
            return new ContentResult
            {
                ContentType = APIControllerBase.JavaScriptContentType,
                ContentEncoding = APIControllerBase.ContentEncoding,
                Content = string.Format("{0}({1});", callback, JsonConvert.SerializeObject(data, Formatting.Indented))
            };
        }

        static string FormatFromAcceptTypes(params string[] acceptTypes)
        {
            if (acceptTypes.Where(t => t.ToLower().Contains(XMLFormat)).Count() > 0)
                return XMLFormat;

            if (acceptTypes.Where(t => t.ToLower().Contains(JSONFormat)).Count() > 0)
                return JSONFormat;

            if (acceptTypes.Where(t => t.ToLower().Contains(JSONPFormat)).Count() > 0)
                return JSONPFormat;

            return JSONFormat;
        }

        public ActionResult SystemInfo()
        {
            Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            return this.APIResult(new { APIVersion = currentVersion.ToString() });
        }
    }
}
