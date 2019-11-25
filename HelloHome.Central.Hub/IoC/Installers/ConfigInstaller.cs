using System;
using System.IO;
using HelloHome.Central.Common.Configuration;
using HelloHome.Central.Repository;
using Lamar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class ConfigInstaller : ServiceRegistry    
    {
        public ConfigInstaller()
        {
            //Config
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

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