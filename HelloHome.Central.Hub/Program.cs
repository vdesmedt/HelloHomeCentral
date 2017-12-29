using Castle.Windsor;
using Castle.MicroKernel.Registration;
using HelloHome.Central.Domain;
using HelloHome.Central.Repository;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HelloHome.Central.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using HelloHome.Central.Hub.MessageChannel;

namespace HelloHome.Central.Hub
{
    public static class Program
    {
        public static void Main(string[] args)
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
                    .Instance(dbCOntextOptionsBuilder.Options),
                Component.For<MessageHub>(),
                Component.For<IMessageChannel>().ImplementedBy<RandomMessageChannel>()
            );

            var hub = ioc.Resolve<MessageHub>();
            var cts = new CancellationTokenSource();
            var t = hub.Process(cts.Token);

            Console.ReadKey();
            Console.WriteLine("Stoping hub...");
            cts.Cancel();

            var l = hub.LeftToProcess;
            while (l > 0)
            {
                Console.WriteLine($"{hub.LeftToProcess} message(s) left to process...");
                while (l == hub.LeftToProcess) ;
                Thread.Sleep(100);
                l = hub.LeftToProcess;
            }

            Console.WriteLine("Processing last message before exiting....");

            t.Wait();

            Console.WriteLine("Done ! Bye bye !");
            ioc.Release(hub);
        }
    }
}