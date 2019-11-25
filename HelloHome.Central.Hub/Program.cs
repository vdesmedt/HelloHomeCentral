using HelloHome.Central.Domain;
using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using HelloHome.Central.Common;
using HelloHome.Central.Hub.IoC.Installers;
using HelloHome.Central.Hub.Logic;
using HelloHome.Central.Hub.MessageChannel;
using Lamar;
using NLog;

namespace HelloHome.Central.Hub
{
    public static class Program
    {
        private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();		

        public static void Main(string[] args)
        {
            var container = Container.For<HubServiceRegistry>();
            Console.WriteLine(container.WhatDoIHave());

            Logger.Info("Starting on machine name : {0}", Environment.MachineName);
            Logger.Info($"Current Dir : {Directory.GetCurrentDirectory()}");

            try
            {
                var hub = container.GetInstance<MessageHub>();
                hub.Start();

                var dbCtx = container.GetInstance<IUnitOfWork>();
                var msgChannel = container.GetInstance<IMessageChannel>();
                new ConsoleApp.ConsoleApp(msgChannel, dbCtx).Run();            
            
                Console.WriteLine("Stopping hub...");
                hub.Stop();

                Console.WriteLine("Done ! Press any key....");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Stopped program because of an exception.");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }                                   
        }
    }
}