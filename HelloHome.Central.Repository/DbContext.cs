using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Repository.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelloHome.Central.Repository
{
    public class HelloHomeContext : DbContext, IUnitOfWork
    {
        public DbSet<Node> Nodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=192.168.1.247;Port=3307;Database=HelloHome_Dev;User Id=hhgtw;Password=othe");
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();
            optionsBuilder.UseLoggerFactory(loggerFactory);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Node>(new NodeConfiguration());
        }
    }
}
