using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Backend.Middleware
{
    public static class RequestCheckManagerExtensions
    {
        public static IApplicationBuilder UseCheckManager(
            this IApplicationBuilder builder)
        {
            return builder.UseWhen(context => context.Request.Path.StartsWithSegments("/api/GeneralInformations/Manager")
                                                || context.Request.Path.StartsWithSegments("/api/Marks/Manager"),
                                    b => b.UseMiddleware<CheckManager>());
        }
    }
    public class CheckManager
    {
        private readonly RequestDelegate _next;
        private readonly BackendContext _context;

        public CheckManager(RequestDelegate next, BackendContext context)
        {
            _next = next;
            _context = context;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            //StreamReader reader = new StreamReader(context.Request.Body,Encoding.UTF8);
            //string datastring = await reader.ReadToEndAsync();
            //Credential cr = JsonConvert.DeserializeObject<Credential>(datastring);

            bool isValid = false;
            if (context.Request.Query.ContainsKey("AccessToken"))
            {
                var cr = await _context.Credential.SingleOrDefaultAsync(c =>
                    c.AccessToken == context.Request.Query["AccessToken"].ToString());
                if (cr != null)
                {
                    var ars = _context.AccountRoles.Where(ar => ar.AccountId == cr.OwnerId);
                    if (ars != null)
                    {
                        foreach (var ar in ars)
                        {
                            if (_context.Role.SingleOrDefault(r => r.RoleId == ar.RoleId).Name == "Manager") // if is manager
                            {
                                isValid = true;
                            }
                        }
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
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(HttpStatusCode.Unauthorized.ToString() + ": Access Denied");
            }
        }
    }
}
