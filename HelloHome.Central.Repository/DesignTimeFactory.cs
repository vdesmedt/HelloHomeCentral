using HelloHome.Central.Repository.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
using HelloHome.Central.Common.Extensions;

namespace HelloHome.Central.Repository
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<HhDbContext>
    {
        public HhDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HhDbContext>();
            optionsBuilder.UseMySql("Server=127.0.0.1;Port=3306;Database=HelloHome_Dev;User Id=hhgtw;Password=othe");

            return new HhDbContext(optionsBuilder.Options);
        }
    }
}
