using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Backend.Middleware
{
    public static class RequestCheckLoginExtensions
    {
        public static IApplicationBuilder UseCheckLogin(
            this IApplicationBuilder builder)
        {
            return builder.UseWhen(context => context.Request.Path.StartsWithSegments("/api/accounts")
                                                || context.Request.Path.StartsWithSegments("/api/GeneralInformations"),
                                    b => b.UseMiddleware<CheckLogin>());
        }
    }
    public class CheckLogin
    {
        private readonly RequestDelegate _next;

        public CheckLogin(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //StreamReader reader = new StreamReader(context.Request.Body,Encoding.UTF8);
            //string datastring = await reader.ReadToEndAsync();
            //Credential cr = JsonConvert.DeserializeObject<Credential>(datastring);

            bool isValid = false;
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                string tokenHeader = context.Request.Headers["Authorization"];
                var token = tokenHeader.Replace("Basic ", "");

                HttpClient client = new HttpClient();
                var responseResult = client.GetAsync("https://localhost:44314/api/Authentication?AccessToken=" + token).Result;
                if (responseResult.StatusCode == HttpStatusCode.OK)
                {
                    isValid = true;
                }
            }

            if (isValid)
            {
                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
            else
            {
                //context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                //await context.Response.WriteAsync("");
                //await context.Response.WriteAsync("Invalid token" + context.Request.Headers.ContainsKey("Authorization"));
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync(HttpStatusCode.Forbidden.ToString());
            }


        }
    }
}
