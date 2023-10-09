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
        T Get(double id);

        Dictionary<double, T> GetAll();

        T Insert(T entity, double id);

        T Update(T entity, double id);

        bool Delete(double id);
    }
}
