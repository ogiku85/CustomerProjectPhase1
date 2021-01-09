using CommonDal;
using FactoryCustomer;
using InterfaceCustomer;
using InterfaceDal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDotNetDal
{
    public abstract class TemplateADO<T> : AbstractDal<T>
    {

        protected SqlConnection objConn = null;
        protected SqlCommand objCommand = null;

        public override void SeUnitOfWork(IUow uow)
        {
            objConn = ((AdoUow)uow).Connection;
            objCommand = new SqlCommand();
            objCommand.Connection = objConn;
            objCommand.Transaction = ((AdoUow)uow).Transaction;
        }
        public TemplateADO(string ConnectionString) : base(ConnectionString)
        {
           // this.ConnectionString = ConnectionString;
        }

        private void Open()
        {
            if (objConn == null)
            {
                objConn = new SqlConnection(ConnectionString);
                objConn.Open();
                objCommand = new SqlCommand();
                objCommand.Connection = objConn;
            }


        }
        protected abstract void ExecuteCommand(T obj);

        protected abstract List<T> ExecuteCommand();
        private void Close()
        {
            objConn.Close();
        }

        //Design Pattern : Template Pattern
        private void Execute(T obj) // Fixed sequence
        {
            Open();
            ExecuteCommand(obj);
            Close();
        }

        private List<T> Execute() // Fixed sequence
        {
            List<T> objTypes = null;
            Open();
           objTypes = ExecuteCommand();
            Close();
            return objTypes;
        }
        public override void Save()
        {
            foreach (var anyType in anyTypes) 
            {
                Execute(anyType);
            }
        }
        public override List<T> Get()
        {
            return Execute();
        }
    }

    public class CustomerDAL : TemplateADO<CustomerBase>
    {
        public CustomerDAL(string ConnectionString) : base(ConnectionString)
        {
            
        }
        public override void Add(CustomerBase obj)
        {
            obj.Validate();
            base.Add(obj);
        }
        protected override void ExecuteCommand(CustomerBase obj)
        {
            objCommand.CommandText = "insert into tblCustomer(CustomerName,BillAmount,BillDate,PhoneNumber,Address) values('" 
                + obj.CustomerName + "'," + obj.BillAmount + ", '" + obj.BillDate + "', '" + obj.PhoneNumber + "', '" + obj.Address + "')";
            objCommand.ExecuteNonQuery();
        }

        protected override List<CustomerBase> ExecuteCommand()
        {
            objCommand.CommandText = "select * from tblCustomer";
            SqlDataReader dr = null;
            dr = objCommand.ExecuteReader();
            List<CustomerBase> custs = new List<CustomerBase>();
            while (dr.Read())
            {
                CustomerBase icust = Factory<CustomerBase>.Create("Customer");
                icust.CustomerName = dr["CustomerName"].ToString();
                icust.BillDate = Convert.ToDateTime(dr["BillDate"]);
                icust.BillAmount = Convert.ToDecimal(dr["BillAmount"]);
                icust.PhoneNumber = dr["PhoneNumber"].ToString();
                icust.Address = dr["Address"].ToString();
               // custs.Add(icust);
                anyTypes.Add(icust);
            }
            return anyTypes;
            //return custs;
        }
    }

    public class AdoUow : IUow
    {
        public SqlConnection Connection { get; set; }
        public SqlTransaction Transaction { get; set; }

        public AdoUow()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            Connection = new SqlConnection(connectionString);
            Connection.Open();
            Transaction = Connection.BeginTransaction();

        }
        public void Commit()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Dispose();
        }
    }
}
