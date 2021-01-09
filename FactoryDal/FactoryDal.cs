using AdoDotNetDal;
using EfDal;
using InterfaceCustomer;
using InterfaceDal;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FactoryDal
{
    public static class FactoryDal<T> // Design pattern :- Simple factory pattern
    {
        private static IUnityContainer ObjectsofOurProjects = null;


        public static T Create(string type)
        {
            // Design pattern :- Lazy loading. Eager loading
            if (ObjectsofOurProjects == null)
            {
                ObjectsofOurProjects = new UnityContainer();

                //ObjectsofOurProjects.RegisterType<IRepository<ICustomer>,
                //    CustomerDAL>("ADODal");

                ObjectsofOurProjects.RegisterType<IRepository<CustomerBase>, CustomerDAL>("ADODal");
                //ObjectsofOurProjects.RegisterType<IRepository<CustomerBase>, EfCustomerDal>("EFDal");
                ObjectsofOurProjects.RegisterType<IRepository<CustomerBase>, EfDalAbstract<CustomerBase>>("EFDal");

                ObjectsofOurProjects.RegisterType<IUow, AdoUow>("AdoUOW");
                ObjectsofOurProjects.RegisterType<IUow, EUow>("EfUOW");
            }
            //should be commented out later
            return ObjectsofOurProjects.Resolve<T>(type, new ResolverOverride[] {
                new ParameterOverride("ConnectionString",@"Data Source=DESKTOP-CUKPDT6\SQLEXPRESS;Initial Catalog=CustomerDB;Integrated Security=True")
            });

           // return ObjectsofOurProjects.Resolve<T>(type);
        }

       
    }
}
