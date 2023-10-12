using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Data.Interface
{
    public interface IRepository<T>
    {
        T Get(long id);

        Dictionary<long, T> GetAll();

        T Insert(T entity, long id);

        T Update(T entity, long id);

        bool Delete(long id);
    }
}
