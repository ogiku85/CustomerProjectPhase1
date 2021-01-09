using InterfaceCustomer;
using InterfaceDal;
using Microsoft.Practices.Unity;
using MiddleLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationAlgorithms;

namespace FactoryCustomer
{
    //Design Pattern: Simple factory pattern
    public static class Factory<T> 
    {
        
        private static Dictionary<string, CustomerBase> customersDict = new Dictionary<string, CustomerBase>();
        private static IUnityContainer projectObjects = null;
        //private static Dictionary<string, CustomerBase> customersDict = new Dictionary<string, CustomerBase> 
        //{
        //    {"Customer", new Customer() },
        //    {"Lead", new Lead() }
        //};

        static Factory()
        {

        }
        public static T Create(string type)
        {
            if (projectObjects == null)
            {
                projectObjects = new UnityContainer();
                IValidation<ICustomer> custValidation = new PhoneValidation(new CustomerBasicValidation());
                //projectObjects.RegisterType<CustomerBase, Customer>("Customer",
                //    new InjectionConstructor(new CustomerValidationAll()));

                projectObjects.RegisterType<CustomerBase, Customer>("Lead",
                    new InjectionConstructor(custValidation, "Lead"));

                custValidation = new CustomerBasicValidation();
                projectObjects.RegisterType<CustomerBase, Customer>("SelfService",
                     new InjectionConstructor(custValidation, "SelfService"));

                custValidation = new CustomerAddressValidation(new CustomerBasicValidation());
                projectObjects.RegisterType<CustomerBase, Customer>("HomeDelivery",
                      new InjectionConstructor(custValidation, "HomeDelivery"));

                custValidation = new PhoneValidation(
                    new CustomerBillValidation(
                    new CustomerAddressValidation(
                    new CustomerBasicValidation()
                    )
                    )
                    );

                projectObjects.RegisterType<CustomerBase, Customer>("Customer",
                     new InjectionConstructor(custValidation, "Customer"));




                //projectObjects.RegisterType<IRepository<ICustomer>, CustomerDAL>("ADODal");
            }

            return projectObjects.Resolve<T>(type);
        }

        public static CustomerBase Create3(string custType)
        {

            /*Design Pattern : Lazy loading only load the dictionary when the create method is calaled and the dictionary is empty*/
            if (customersDict.Count == 0)
            {
                //customersDict.Add("Customer", new Customer());
                //customersDict.Add("Lead", new Lead());
            }

            //Design Pattern : RIP
            /*
             This method uses RIP pattern to replace IF patterns. RIP means Replace If with Polymorphism
             */
            return customersDict[custType];
        }
        //public static CustomerBase Create2(string custType)
        //{
        //    if (custType == "Customer")
        //    {
        //        return new Customer();
        //    }
        //    else
        //    {
        //        return new Lead();
        //    }
        //}
    }

    public static class FactoryOLD
    {

        private static Dictionary<string, CustomerBase> customersDict = new Dictionary<string, CustomerBase>();
        private static IUnityContainer customersUnityContainer = null;
        //private static Dictionary<string, CustomerBase> customersDict = new Dictionary<string, CustomerBase> 
        //{
        //    {"Customer", new Customer() },
        //    {"Lead", new Lead() }
        //};

        static FactoryOLD()
        {

        }
        public static ICustomer Create(string custType)
        {
            //if (customersUnityContainer == null)
            //{
            //    customersUnityContainer = new UnityContainer();
            //    customersUnityContainer.RegisterType<ICustomer, Customer>("Customer",
            //        new InjectionConstructor(new CustomerValidationAll()));
            //    customersUnityContainer.RegisterType<ICustomer, Lead>("Lead",
            //        new InjectionConstructor(new LeadValidation()));
            //}

            return customersUnityContainer.Resolve<ICustomer>(custType);
        }

        public static CustomerBase Create3(string custType)
        {

            /*Design Pattern : Lazy loading only load the dictionary when the create method is calaled and the dictionary is empty*/
            if (customersDict.Count == 0)
            {
                //customersDict.Add("Customer", new Customer());
                //customersDict.Add("Lead", new Lead());
            }

            //Design Pattern : RIP
            /*
             This method uses RIP pattern to replace IF patterns. RIP means Replace If with Polymorphism
             */
            return customersDict[custType];
        }
        //public static CustomerBase Create2(string custType)
        //{
        //    if (custType == "Customer")
        //    {
        //        return new Customer();
        //    }
        //    else
        //    {
        //        return new Lead();
        //    }
        //}
    }
}
