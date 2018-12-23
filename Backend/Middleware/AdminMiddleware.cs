using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Backend.Middleware
{
    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseACheckAdmin(
            this IApplicationBuilder builder)
        {
           return builder.UseWhen(context => context.Request.Path.StartsWithSegments("/api/roles"),b => b.UseMiddleware<AdminMiddleware>());
        }
    }
    public class AdminMiddleware
    {

        private readonly RequestDelegate _next;

        public AdminMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            bool isValid = false;
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                //var rawToken = context.Request.B
                //rawToken = rawToken.Replace("Basic ", "");
                //HttpClient client = new HttpClient();
                //var responseResult = client.GetAsync("https://toauth2server.azurewebsites.net/api/authentication?accessToken=" + rawToken).Result;
                //if (responseResult.StatusCode == HttpStatusCode.OK)
                //{
                //    isValid = true;
                //}
            }
            if (isValid)
            {
                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Invalid token" + context.Request.Headers.ContainsKey("Authorization"));
            }


        }
    }
}
