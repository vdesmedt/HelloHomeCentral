using HelloHome.Central.Domain;
using HelloHome.Central.Repository;
using Lamar;

namespace HelloHome.Central.Common.IoC.Registries
{
    public class DbContextRegistry : ServiceRegistry
    {
        public DbContextRegistry()
        {
            For<IUnitOfWork>().Use<HhDbContext>().Scoped();
        }
    }
}