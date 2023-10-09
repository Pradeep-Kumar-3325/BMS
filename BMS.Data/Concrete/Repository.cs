using BMS.Data.Interface;
using Microsoft.Extensions.Logging;

namespace BMS.Data.Concrete
{
    public class Repository<T> : IRepository<T>
    {
        // For Real application we should use RDMS or NOSQL.
        // The below for just assignment and for mock
        private Dictionary<double, T> table;

        private readonly ILogger<Repository<T>> _logger;

        public Repository(ILogger<Repository<T>> logger)
        {
            this._logger = logger;
            table = BMDDatabase.GetTable<T>();
        }
        public bool Delete(double id)
        {
            try
            {
                return table.Remove(id);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Delete :- {ex}");
                throw;
            }
        }
        public T Get(double id)
        {
            try
            {
                return table.Where(x => x.Key == id).Select(x => x.Value).FirstOrDefault();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Get :- {ex}");
                throw;
            }
        }

        public Dictionary<double, T> GetAll()
        {
            try
            {
                return table;
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in GetAll :- {ex}");
                throw;
            }
        }

        public T Insert(T entity, double id)
        {
            try
            {
                table.Add(id, entity);
                return entity;
            }

            catch (Exception ex)
            {
                this._logger.LogError($"Error in Insert :- {ex}");
                throw;
            }
        }

        public T Update(T entity, double id)
        {
            try
            {
                table[id] = entity;
                return entity;
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Update :- {ex}");
                throw;
            }
        }
    }
}