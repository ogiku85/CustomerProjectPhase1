using InterfaceCustomer;
using InterfaceDal;
using MiddleLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfDal
{
    /*Design Pattern : Adapter Pattern i.e Class Adapter Pattern By using IRepository<T> the clients or consumers
     are proetcted from the fact that we are using ADO .Net or EntityFramework they just code to the IRepository intefcae so
     we can use ExceuteNonQuery for ADO .net or savechanegs in enity framework when they call the save method in IRepository<T>
         */
    public class EfDalAbstract<T> :  IRepository<T> where T : class
    {
        DbContext Context = null;
        public EfDalAbstract() 
        {
            Context = new EUow();
        }
        public void Add(T obj)
        {
            Context.Set<T>().Add(obj);
        }

        public List<T> Get()
        {
            return Context.Set<T>().AsQueryable<T>().ToList<T>();
        }

        public List<T> GetInMemoryData()
        {
            throw new NotImplementedException();
        }

        public T GetInMemoryData(int index)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public void SeUnitOfWork(IUow uow)
        {
            Context = ((EUow)uow);
        }

        public void Update(T obj)
        {
            throw new NotImplementedException();
        }
    }

    public class EfDalAbstractOLD<T> : DbContext, IRepository<T> where T : class
    {
        public EfDalAbstractOLD() : base("name=Conn")
        {

        }
        public void Add(T obj)
        {
            Set<T>().Add(obj);
        }

        public List<T> Get()
        {
            return Set<T>().AsQueryable<T>().ToList<T>();
        }

        public List<T> GetInMemoryData()
        {
            throw new NotImplementedException();
        }

        public T GetInMemoryData(int index)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            SaveChanges();
        }

        public void SeUnitOfWork(IUow uow)
        {
            throw new NotImplementedException();
        }

        public void Update(T obj)
        {
            throw new NotImplementedException();
        }
    }
    //public class EfCustomerDal : EfDalAbstract<CustomerBase>
    //{
    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        //base.OnModelCreating(modelBuilder);

    //        //mapping Code
    //        modelBuilder.Entity<CustomerBase>().ToTable("tblCustomer");
    //        //modelBuilder.Entity<Customer>().ToTable("tblCustomer");
    //        //modelBuilder.Entity<Lead>().ToTable("tblCustomer");

    //        //modelBuilder.Entity<CustomerBase>().Map<Customer>(m => m.Requires("CustomerType").HasValue("Customer"));
    //        //modelBuilder.Entity<CustomerBase>().Map<Lead>(m => m.Requires("CustomerType").HasValue("Lead"));

    //        modelBuilder.Entity<CustomerBase>().Ignore(t => t.CustomerType);
    //    }
    //}

    public class EUow : DbContext, IUow
    {
        public DbContext Context { get; set; }

        public EUow()
        {
            Context = new DbContext("name=Conn");
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //mapping Code
            modelBuilder.Entity<CustomerBase>().ToTable("tblCustomer");
        }
        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Rollback()
        {
            Context.Dispose();
        }
    }
}
