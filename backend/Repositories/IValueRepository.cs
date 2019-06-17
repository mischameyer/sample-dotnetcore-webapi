using System.Collections.Generic;
using System.Linq;
using Backend.Entities;

namespace Backend.Repositories
{
    public interface IValueRepository
    {
        Value GetSingle(int id);
        void Add(Value item);
        void Delete(int id);
        Value Update(int id, Value item);
        IQueryable<Value> GetAll();
        int Count();

        bool Save();
    }
}
