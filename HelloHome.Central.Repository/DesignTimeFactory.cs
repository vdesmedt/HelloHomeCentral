using HelloHome.Central.Repository.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
using HelloHome.Central.Common.Extensions;

namespace HelloHome.Central.Repository
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<HelloHomeDbContext>
    {
        public HelloHomeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HelloHomeDbContext>();
            optionsBuilder.UseMySql("Server=127.0.0.1;Port=3306;Database=HelloHome_Dev;User Id=hhgtw;Password=othe");

            return new HelloHomeDbContext(optionsBuilder.Options);
        }
    }
}
