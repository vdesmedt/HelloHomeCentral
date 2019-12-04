using System;
using System.Diagnostics;
using System.IO;
using HelloHome.Central.Common.Configuration;
using HelloHome.Central.Repository;
using Lamar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class ConfigInstaller : ServiceRegistry    
    {
        private static readonly Logger Logger = NLog.LogManager.GetLogger(nameof(ConfigInstaller));		

        public ConfigInstaller()
        {
            //Config
            var s = Stopwatch.StartNew();
            var configBuilder = new ConfigurationBuilder();
            Logger.Debug($"Config builder created after {s.ElapsedMilliseconds} ms");
            configBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile($"appsettings.json", false,  true);
            configBuilder.AddJsonFile($"appsettings.{Environment.MachineName}.json", true,  true);
            configBuilder.AddEnvironmentVariables();
            var config = configBuilder.Build();
            
            For<IConfiguration>().Use(config);
            For<SerialConfig>().Use(config.GetSection("Serial").Get<SerialConfig>()).Singleton();
            For<EmonCmsConfig>().Use(config.GetSection("EmonCms").Get<EmonCmsConfig>()).Singleton();
            
            //DbContextOption
            var cnString = config.GetValue<string>("ConnectionString");
            var dbCtxOptionBuilder = new DbContextOptionsBuilder<HhDbContext>();
            dbCtxOptionBuilder.UseMySql(cnString);
            dbCtxOptionBuilder.UseLoggerFactory(new LoggerFactory());
            For<DbContextOptions<HhDbContext>>().Use(dbCtxOptionBuilder.Options);
        }
    }
}