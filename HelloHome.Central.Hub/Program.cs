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
using Castle.Windsor.Installer;
using HelloHome.Central.Common.Configuration;
using HelloHome.Central.Hub.IoC.Installers;
using HelloHome.Central.Hub.MessageChannel;
using NLog;

namespace HelloHome.Central.Hub
{
    public static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();		

        public static void Main(string[] args)
        {
            Logger.Info("Starting on machine name : {0}", Environment.MachineName);
            var configRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appconfig.json", optional:false, reloadOnChange:true)
                .AddJsonFile($"appconfig.{Environment.MachineName}.json", optional:true, reloadOnChange:true)
                .AddEnvironmentVariables()
                .Build();
            var config = new HhConfig();
            configRoot.Bind(config);            
            
            var ioc = new WindsorContainer();
            ioc.Install(                
                new ConfigInstaller(config),
                new FacilityInstaller(),
                new HubInstaller(),
                new HandlerInstaller(),
                new BusinessLogicInstaller(),
                new CommandAndQueriesInstaller(),
                new MessageChannelInstaller(),
                new DbContextInstaller(config.ConnectionString)
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

            Console.WriteLine("Done ! Press any key....");
            Console.ReadKey();
            ioc.Release(hub);
        }
    }
}