using backend.Core;
using backend.Mapper;
using Backend.Repositories;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend
{
    public class Startup
    {

        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {                
            }).AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            }); ;

            services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();            

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend.Api", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("default", builder =>
                {
                    List<string> origins = new List<string> {                        
                        "http://localhost:4200"
                    };

                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins(origins.ToArray())
                        .WithExposedHeaders("Content-Disposition");
                });
            });
            
            services.AddAutoMapper(typeof(ProfileValueMapper));
            services.AddDbContext<ValueDbContext>(opt => opt.UseInMemoryDatabase("ValueDatabase"));
            services.AddScoped<IValueRepository, ValueRepository>();
            
            services.AddHttpContextAccessor();
            services.AddRouting(options => options.LowercaseUrls = true);            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend.Api v1"));
            }
            else
            {
                app.UseExceptionHandlerTracing();
            }

            app.UseCors("default");
            app.UseHttpsRedirection();

            app.UseAuthentication();            

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
