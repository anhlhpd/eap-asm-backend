using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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
                    Role role = JsonConvert.DeserializeObject<Role>(responseResult.Content.ReadAsStringAsync().Result);
                    //await context.Response.WriteAsync(role.ToString());
                    //return;
                    if (role.Name == "Admin")
                    {
                        isValid = true;
                    }
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
