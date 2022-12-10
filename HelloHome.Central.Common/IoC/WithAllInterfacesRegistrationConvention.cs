using System.Diagnostics;
using System.Linq;
using JasperFx.TypeDiscovery;
using Lamar;
using Lamar.Scanning.Conventions;

namespace HelloHome.Central.Common.IoC
{
        public class WithAllInterfacesRegistrationConvention : IRegistrationConvention
        {
            public void ScanTypes(TypeSet types, ServiceRegistry services)
            {
                if (types.Records.Count() > 1)
                {
                    Debug.WriteLine("");
                }

                foreach (var type in types.FindTypes(TypeClassification.Concretes | TypeClassification.Closed))
                {
                    // Register against all the interfaces implemented
                    // by this concrete class
                    foreach (var @interface in type.GetInterfaces())
                    {
                        services.For(@interface).Use(type).Scoped();
                    }

                    services.For(type.BaseType).Use(type);
                    services.For(type).Use(type);
                };
            }
        }
    }
