using Castle.Windsor;
using Castle.MicroKernel.Registration;
using HelloHome.Central.Domain;
using HelloHome.Central.Repository;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HelloHome.Central.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HelloHome.Central.Hub
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appconfig.json")
                .Build();

            var ioc = new WindsorContainer();

            var dbCOntextOptionsBuilder = new DbContextOptionsBuilder<HelloHomeContext>();

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();

            dbCOntextOptionsBuilder
                .UseMySql(config.GetConnectionString("Default"))
                .UseLoggerFactory(loggerFactory);

            ioc.Register(
                Component.For<IUnitOfWork>()
                    .ImplementedBy<HelloHomeContext>()
                    .LifestyleSingleton(),
                Component.For<DbContextOptions<HelloHomeContext>>()
                    .Instance(dbCOntextOptionsBuilder.Options)                    
            );

            var ctx = ioc.Resolve<IUnitOfWork>();

            ctx.Nodes.Add(new Domain.Entities.Node { RfAddress = 1, Signature = long.MaxValue, LastSeen = DateTime.Now, LastStartupTime = DateTime.Now, Metadata = new NodeMetadata() });
            ctx.SaveChanges();

            var nodes = ctx.Nodes;
            Console.WriteLine($"Node count:{nodes.ToList().Count}");
            ioc.Release(ctx);
        }
    }
}
