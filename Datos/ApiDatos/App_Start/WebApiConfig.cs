using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNet.WebApi.Extensions.Compression.Server;
using System.Net.Http.Extensions.Compression.Core.Compressors;
using ApiDatos.Handlers;

namespace ApiDatos
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Enabled CORS
            config.EnableCors();

            //COnfig MessageHandler Security
            config.MessageHandlers.Add(new BasicAuthMessageHandler());

            // Web API configuration and services (Config JSON Response)
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(config.Formatters.JsonFormatter);
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Insert(0, config.Formatters.JsonFormatter);

            //Config MessageHandlers Compression
            config.MessageHandlers.Insert(0, new ServerCompressionHandler(200 * 1024, new GZipCompressor(), new DeflateCompressor()));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
