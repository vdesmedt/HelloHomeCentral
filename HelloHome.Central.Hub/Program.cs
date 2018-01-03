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
using HelloHome.Central.Hub.IoC.Installers;
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
            ioc.Install(                
                new FacilityInstaller(),
                new HubInstaller(),
                new MessageChannelInstaller(),
                new DbContextInstaller(config)
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