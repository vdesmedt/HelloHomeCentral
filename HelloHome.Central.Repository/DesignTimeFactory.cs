using System;
using HelloHome.Central.Repository.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using HelloHome.Central.Common.Extensions;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace HelloHome.Central.Repository
{
    public class DesignTimeFactoryDev : IDesignTimeDbContextFactory<HhDbContext>
    {
        public HhDbContext CreateDbContext(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            builder.AddJsonFile("migrationsettings.json");
            builder.AddEnvironmentVariables();
            var configuration = builder.Build();
            var env = configuration["HHEnv"]??"dev";
            Console.WriteLine($"Environment being used:{env}");
            Console.WriteLine(configuration.GetConnectionString(env));
            var optionsBuilder = new DbContextOptionsBuilder<HhDbContext>();
            optionsBuilder.UseMySql(configuration.GetConnectionString(env), optionBuilder =>
            {
                optionBuilder.ServerVersion(new Version(10, 4, 11), ServerType.MariaDb);
            });

            return new HhDbContext(optionsBuilder.Options);
        }
    }
}
