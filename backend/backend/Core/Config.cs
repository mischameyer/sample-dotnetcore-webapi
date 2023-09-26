namespace backend.Core
{
    public static class Config
    {
        public const string EnvironmentNameLocal = "Local";
        public const string EnvironmentVariablePrefix = "Backend_Api_";
    }

    public static class ServiceConfigurationExtensions
    {
        public static bool IsLocal(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("Settings:Environment").Equals(Config.EnvironmentNameLocal, StringComparison.OrdinalIgnoreCase);
        }

        public static string GetEvaluationProcessorHostServerFilter(this IConfiguration configurationRoot)
        {
            return configurationRoot.GetValue<string>("Evaluation:ProcessorHostServerFilter")?.Trim();
        }
    }

    public static class HostEnvironmentExtensions
    {
        public static bool IsWebHost(this IHostEnvironment hostEnvironment)
        {
            return string.Equals(hostEnvironment.ApplicationName, "backend.Api", StringComparison.OrdinalIgnoreCase);
        }
    }

}
