using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Repository.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;

namespace HelloHome.Central.Repository
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<HelloHomeContext>
    {
        public HelloHomeContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HelloHomeContext>();
            optionsBuilder.UseMySql("Server=127.0.0.1;Port=3306;Database=HelloHome_Dev;User Id=hhgtw;Password=othe");

            return new HelloHomeContext(optionsBuilder.Options);
        }
    }

    public class HelloHomeContext : DbContext, IUnitOfWork
    {
        public HelloHomeContext(DbContextOptions<HelloHomeContext> options)
            : base(options) { }

        public DbSet<Node> Nodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Node>(new NodeConfig());
            modelBuilder.ApplyConfiguration<NodeMetadata>(new NodeMetaConfig());
        }

        int IUnitOfWork.SaveChanges()
        {
            return SaveChanges();
        }
    }
}
