﻿using HelloHome.Central.Domain;
using HelloHome.Central.Repository;
using Lamar;

namespace HelloHome.Central.WebAPI.IoC
{
    public class DbContextInstaller : ServiceRegistry
    {
        public DbContextInstaller()
        {
            For<IUnitOfWork>().Use<HhDbContext>().Scoped();
        }
    }
}