using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Swan.Logging;
using System;
using System.IO;
using System.Net;

namespace WS
{
    public static class LocalWebserver
    {
        public static WebServer CreateWebServer(string url)
        {
            try
            {
                var server = new WebServer(o => o
                    .WithUrlPrefix(url))
                //.WithMode(HttpListenerMode.EmbedIO))
                // First, we will configure our web server by adding Modules.
                //.WithLocalSessionManager()
                .WithWebApi("/api", m => m
                    .WithController<TestRoute>());

                // Listen for state changes.
                server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();

                return server;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

    public class TestRoute : WebApiController
    {
        [Route(HttpVerbs.Get, "/test")]
        public string GetMe(WebServer server,
                            HttpListenerContext context)
        {
            try
            {
                //StreamReader reader = new StreamReader(context.Request.InputStream);
                //string userJson = reader.ReadToEnd();

                //return userJson;
                return "Hello Jesper";

            }
            catch (Exception ex)
            {
                return HandleError(context, ex,
                                   (int)HttpStatusCode.InternalServerError);
            }
        }

        [Route(HttpVerbs.Post, "/test")]
        public string PostMe(WebServer server,
                            HttpListenerContext context)
        {
            try
            {
                StreamReader reader = new StreamReader(context.Request.InputStream);
                string userJson = reader.ReadToEnd();

                return userJson;

            }
            catch (Exception ex)
            {
                return HandleError(context, ex,
                                   (int)HttpStatusCode.InternalServerError);
            }
        }

        private string HandleError(HttpListenerContext context,
                           Exception ex,
                           int statusCode = 500)
        {
            return ex.Message;
        }
    }
}
