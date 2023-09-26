using Microsoft.AspNetCore.Diagnostics;
using NLog;

namespace backend.Core
{
    internal static class ExceptionHandlerMiddlewareExtensions
    {
        private static readonly NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public static void UseExceptionHandlerTracing(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/plain";
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorFeature != null)
                    {                        
                        logger.Error(errorFeature.Error);
                    }

                    await context.Response.WriteAsync("There was an error");
                });
            });
        }
    }
}
