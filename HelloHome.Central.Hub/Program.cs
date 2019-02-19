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
using HelloHome.Central.Hub.WebAPI;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog;

namespace HelloHome.Central.Hub
{
    public static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();		

        public static void Main(string[] args)
        {

            Logger.Info("Starting on machine name : {0}", Environment.MachineName);
            Logger.Info($"Current Dir : {Directory.GetCurrentDirectory()}");
                       
            var webHostTask = CreateWebHostBuilder(args).Build().RunAsync();
            webHostTask.Wait();
                        
            var hub = Startup.IoCContainer.Resolve<MessageHub>();
            hub.Start();
            
            var webApiTask = CreateWebHostBuilder(args).Build().RunAsync();
            webApiTask.Wait();            

            var dbCtx = Startup.IoCContainer.Resolve<IUnitOfWork>("TransientDbContext");
            var msgChannel = Startup.IoCContainer.Resolve<IMessageChannel>();
            new ConsoleApp.ConsoleApp(msgChannel, dbCtx).Run();            
            
            Console.WriteLine("Stopping hub...");
            hub.Stop();
            Startup.IoCContainer.Release(hub);

            Console.WriteLine("Done ! Press any key....");
            Console.ReadKey();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}