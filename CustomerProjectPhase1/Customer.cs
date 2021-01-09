using InterfaceCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddleLayer
{

    public class Customer: CustomerBase
    {
        public Customer()
        {
            CustomerType = "Customer";
        }
        public Customer(IValidation<ICustomer> validation, string custType):base(validation)
        {
            CustomerType = custType;
        }
    }
    //public class Lead: CustomerBase
    //{
    //    public Lead()
    //    {
    //        CustomerType = "Lead";
    //    }
    //    public Lead(IValidation<ICustomer> validation) : base(validation)
    //    {

    //    }
    //}
}
