using Backend.Entities;
using Backend.Repositories;
using Backend.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace backend
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
            services.AddDbContext<ValueDbContext>(opt => opt.UseInMemoryDatabase("ValueDatabase"));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddScoped<IValueRepository, ValueRepository>();
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Sample of simple ASP.NET Core WebAPI", Version = "V1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            loggerFactory.AddConsole();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Version 1");                                  
                });

            }
            else
            {
                app.UseHsts();
                 app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/plain";
                        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (errorFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500, errorFeature.Error, errorFeature.Error.Message);
                        }

                        await context.Response.WriteAsync("There was an error");
                    });
                });
            }
           
            app.UseHttpsRedirection();
            app.UseCors("AllowAllOrigins");

            AutoMapper.Mapper.Initialize(mapper =>
            {
                mapper.CreateMap<Value, ValueDto>().ReverseMap();
                mapper.CreateMap<Value, ValueUpdateDto>().ReverseMap();
                mapper.CreateMap<Value, ValueCreateDto>().ReverseMap();
            });

            app.UseMvc();
        }
    }
}
