using Serilog.Filters;
using Serilog;

namespace JayRide.Test.Api.Core.Logging
{
    public static class LoggingIHostExtension
    {
        public static ConfigureHostBuilder TryUseSerilogFileLogging(this ConfigureHostBuilder host, string filePath)
        {
            host.UseSerilog((ctx, lc) => lc
                        .Enrich.FromLogContext()
                        .AddMinimumLevelOverrides()
                        .AddFilterByExcluding()
                        .WriteTo.Console()
                        .WriteTo.File(filePath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true));
            
            return host;
        }
        private static LoggerConfiguration AddMinimumLevelOverrides(this LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.FileStreamResultExecutor", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor", Serilog.Events.LogEventLevel.Warning); 
            
            return loggerConfiguration;
        }
        private static LoggerConfiguration AddFilterByExcluding(this LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
            .Filter.ByExcluding(Matching.FromSource("CorrelationId.CorrelationIdMiddleware"))
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware"))
            .Filter.ByExcluding(Matching.FromSource("Marvin.Cache.Headers.HttpCacheHeadersMiddleware"))
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware"))
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.ResponseCaching.ResponseCachingMiddleware"))
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager"))
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Routing.EndpointMiddleware"));

            return loggerConfiguration;
        }
    }

}
