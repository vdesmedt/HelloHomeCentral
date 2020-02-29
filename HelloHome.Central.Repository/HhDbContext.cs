﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelloHome.Central.Repository
{
    public class HhDbContext : DbContext, IUnitOfWork
    {
        public HhDbContext(DbContextOptions<HhDbContext> options)
            : base(options) { }

        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodeHistory> NodeHistory { get; set; }
        public DbSet<Trigger> Triggers { get; set; }
        public DbSet<NodeLog> NodeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var configurationTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x =>
                    IntrospectionExtensions.GetTypeInfo(x).IsClass == true 
                    && IntrospectionExtensions.GetTypeInfo(x).IsAbstract == false 
                    && x.GetInterfaces().Any(y => 
                        IntrospectionExtensions.GetTypeInfo(y).IsGenericType == true
                        && y.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
                    )
                );

            var configurations = configurationTypes.Select(Activator.CreateInstance);

            foreach (dynamic configuration in configurations)
            {
                modelBuilder.ApplyConfiguration(configuration);
            }                        
        }

        int IUnitOfWork.Commit()
        {
            return SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return SaveChangesAsync();
        }
    }
}