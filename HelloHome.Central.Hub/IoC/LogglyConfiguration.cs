using Loggly;
using Loggly.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelloHome.Central.Hub.IoC
{
    public class LogglyConfiguration
    {
        public string CustomerToken { get; set; }
    }
    public static class LogglyServiceConfiguration
    {
        public static void ConfigureLoggly(this IServiceCollection services, IConfiguration configuration )
        {
            var logglyConfig = new LogglyConfiguration();
            configuration.GetSection("Loggly").Bind(logglyConfig);

            var config = LogglyConfig.Instance;
            config.CustomerToken = logglyConfig.CustomerToken;
            config.ApplicationName = $"HelloHomeCentral";

            config.Transport.EndpointHostname = "logs-01.loggly.com";
            config.Transport.EndpointPort = 443;
            config.Transport.LogTransport = LogTransport.Https;

            var ct = new ApplicationNameTag();
            ct.Formatter = "AspNetCoreNlogLogglyTutorial";
            config.TagConfig.Tags.Add(ct);
        }
    }}