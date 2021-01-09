using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceDal
{
    //Design Pattern : Generic Repository
    public interface IRepository<T>
    {
        void SeUnitOfWork(IUow uow);
        void Add(T obj);
        void Update(T obj);
        List<T> Get();
        List<T> GetInMemoryData();
        T GetInMemoryData(int index);
        void Save();
    }

    //Design Pattern Unit Of Work
    public interface IUow 
    {
        void Commit();
        void Rollback();
    }

}
