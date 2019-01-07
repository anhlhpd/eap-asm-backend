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
                                                || context.Request.Path.StartsWithSegments("/api/GeneralInformations")
                                                || context.Request.Path.StartsWithSegments("/api/Marks")
                                                || context.Request.Path.StartsWithSegments("/api/Subjects")
                                                || context.Request.Path.StartsWithSegments("/api/Clazz")
                                                || context.Request.Path.StartsWithSegments("/api/ClazzSubjects")
                                                || context.Request.Path.StartsWithSegments("/api/ClazzAccounts")
                                                || context.Request.Path.StartsWithSegments("/api/AccountRoles")
                                                || context.Request.Path.StartsWithSegments("/api/Roles"),
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
                var responseResult = client.GetAsync("https://"+context.Request.Host.Value+"/api/Authentication?AccessToken=" + token).Result;
                if (responseResult.StatusCode == HttpStatusCode.OK)
                {
                    isValid = true;
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync(HttpStatusCode.Unauthorized.ToString() + "Your Token is expired. Please login again!");
                    return;
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
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(HttpStatusCode.Unauthorized.ToString() + ": Access Denied");
            }


        }
    }
}
