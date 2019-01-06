using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Data;

namespace Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAny",
                    builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            });
            services.AddMvc().AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddDbContext<BackendContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("BackendContext")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            
            app.UseHttpsRedirection();
            app.UseCors("AllowAny");
            app.UseCheckAdmin();
            app.UseCheckLogin();
            app.UseMvc();

            //using (var s = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    var context = s.ServiceProvider.GetService<BackendContext>();
            //    context.Database.Migrate();
            //    context.Initialize();
            //}
        }
    }
}
