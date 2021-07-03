using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cdy.Spider.WebApiServer
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

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/{name}", async ctx => {
                    var name = ctx.Request.RouteValues["name"];
                    string res = "";
                    if (name != null)
                    {
                        if (ctx.Request.ContentType == "application/json")
                        {
                            try
                            {
                                string str = new System.IO.StreamReader(ctx.Request.Body, System.Text.Encoding.UTF8).ReadToEndAsync().Result;
                                var dd = ServiceMapManager.Manager.GetService(name.ToString());
                                if (dd != null)
                                {
                                    res = dd(str);
                                }
                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            res = "ContentType is not application/json";
                        }
                    }
                    if (!string.IsNullOrEmpty(res))
                    {
                        ctx.Response.ContentType = "application/json";
                        await ctx.Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes(res));
                    }
                });
            });
        }
    }
}
