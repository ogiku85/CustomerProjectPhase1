using InterfaceCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationAlgorithms
{
    public class CustomerBasicValidation : IValidation<ICustomer>
    {
        public void Validate(ICustomer obj)
        {
            if (obj.CustomerName.Length == 0)
            {
                throw new Exception("Customer Name is required.");
            }
            
        }
    }

    public class ValidationLinker : IValidation<ICustomer>
    {
        private IValidation<ICustomer> nextValidator = null;
        public ValidationLinker(IValidation<ICustomer> nextValidator)
        {
            this.nextValidator = nextValidator;
        }
        public virtual void Validate(ICustomer obj)
        {
            nextValidator.Validate(obj);
        }
    }

    public class PhoneValidation : ValidationLinker
    {
        public PhoneValidation(IValidation<ICustomer> nextValidator) : base(nextValidator)
        {
        }

        public override void Validate(ICustomer obj)
        {
            base.Validate(obj);
            if (obj.PhoneNumber.Length == 0)
            {
                throw new Exception("Phone Number is required.");
            }

        }
    }

    public class CustomerBillValidation : ValidationLinker
    {
        public CustomerBillValidation(IValidation<ICustomer> nextValidator) : base(nextValidator)
        {
        }

        public override void Validate(ICustomer obj)
        {
            base.Validate(obj);
            if (obj.BillAmount == 0)
            {
                throw new Exception("Bill Amount is required.");
            }
            if (obj.BillDate >= DateTime.Now)
            {
                throw new Exception("Bill Date can not be greater than now");
            }

        }
    }


    public class CustomerAddressValidation : ValidationLinker
    {
        public CustomerAddressValidation(IValidation<ICustomer> nextValidator) : base(nextValidator)
        {
        }

        public override void Validate(ICustomer obj)
        {
            base.Validate(obj);
            if (obj.Address.Length == 0)
            {
                throw new Exception("Address is required.");
            }

        }
    }

    //public class CustomerValidationAll : IValidation<ICustomer>
    //{
    //    public void Validate(ICustomer obj)
    //    {
    //        if (obj.CustomerName.Length == 0)
    //        {
    //            throw new Exception("Customer Name is required.");
    //        }
    //        if (obj.PhoneNumber.Length == 0)
    //        {
    //            throw new Exception("Phone Number is required.");
    //        }
    //        if (obj.BillAmount == 0)
    //        {
    //            throw new Exception("Bill Amount is required.");
    //        }
    //        if (obj.BillDate >= DateTime.Now)
    //        {
    //            throw new Exception("Bill Date can not be greater than now");
    //        }
    //        if (obj.Address.Length == 0)
    //        {
    //            throw new Exception("Address is required.");
    //        }
    //    }
    //}

    //public class LeadValidation : IValidation<ICustomer>
    //{
    //    public void Validate(ICustomer obj)
    //    {
    //        if (obj.CustomerName.Length == 0)
    //        {
    //            throw new Exception("Customer Name is required.");
    //        }
    //        if (obj.PhoneNumber.Length == 0)
    //        {
    //            throw new Exception("Phone Number is required.");
    //        }
    //    }
    //}
}
