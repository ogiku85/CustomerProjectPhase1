using InterfaceDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDal
{
    public abstract class AbstractDal<T> : IRepository<T>
    {
        protected string ConnectionString = "";
        protected List<T> anyTypes = new List<T>();
        public AbstractDal(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }
        public virtual void Add(T obj)
        {
            foreach (T temp in anyTypes)
            {
                if (obj.Equals(temp))
                {
                    return;
                }
            }
            anyTypes.Add(obj);
        }

        public virtual List<T> Get()
        {
            throw new NotImplementedException();
        }

        public List<T> GetInMemoryData()
        {
            return anyTypes;
        }

        public T GetInMemoryData(int index)
        {
            return anyTypes[index];
        }

        public virtual void Save()
        {
            throw new NotImplementedException();
        }

        public virtual void SeUnitOfWork(IUow uow)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
