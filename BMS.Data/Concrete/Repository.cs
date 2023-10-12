using BMS.Data.Interface;
using Microsoft.Extensions.Logging;

namespace BMS.Data.Concrete
{
    public class Repository<T> : IRepository<T>
    {
        // For Real application we should use RDMS or NOSQL.
        // The below for just assignment and for mock
        private Dictionary<long, T> table; //Dictionary<double, T> table;

        private readonly ILogger<Repository<T>> _logger;

        public Repository(ILogger<Repository<T>> logger)
        {
            this._logger = logger;
            
        }
        public bool Delete(long id)
        {
            try
            {
                //return table.Remove(id);
                return BMDDatabase.Remove<T>(id);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Delete :- {ex}");
                throw;
            }
        }

        public T Get(long id)
        {
            try
            {
                table = BMDDatabase.GetTable<T>();
                return table.Where(x => x.Key == id).Select(x => x.Value).FirstOrDefault();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Get :- {ex}");
                throw;
            }
        }

        public Dictionary<long, T> GetAll()
        {
            try
            {
                table = BMDDatabase.GetTable<T>();
                return table;
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in GetAll :- {ex}");
                throw;
            }
        }

        public T Insert(T entity, long id)
        {
            try
            {
                // table = BMDDatabase.GetTable<T>();
                BMDDatabase.Add(entity, id);
                return entity;
            }

            catch (Exception ex)
            {
                this._logger.LogError($"Error in Insert :- {ex}");
                throw;
            }
        }

        public T Update(T entity, long id)
        {
            try
            {
                BMDDatabase.Update(entity, id);
                //table[id] = entity;
                //table = BMDDatabase.GetTable<T>();
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