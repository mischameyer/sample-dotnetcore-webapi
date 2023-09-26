using backend.Core;
using NLog;
using NLog.Web;

namespace backend
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                logger.Error($"**** UNHANDLED EXCEPTION ****\r\n {exception}");
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddEnvironmentVariables(prefix: Config.EnvironmentVariablePrefix);
            }).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
                webBuilder.CaptureStartupErrors(true);
                webBuilder.UseStartup<Startup>();

            }).ConfigureLogging((hostingContext, logging) =>
            {
                logging.ClearProviders();

            }).UseNLog();
        }
    }
}
