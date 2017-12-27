using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
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
            optionsBuilder.UseMySQL("Server=192.168.1.247;Port=3306;Database=HelloHome_Dev;User Id=hhgtw;Password=othe");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
