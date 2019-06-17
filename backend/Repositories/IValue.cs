using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Entities;

namespace Backend.Repositories
{
    public class ValueRepository : IValueRepository
    {
        private readonly ValueDbContext _dbContext;

        public ValueRepository(ValueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Value GetSingle(int id)
        {
            return _dbContext.Values.FirstOrDefault(x => x.Id == id);
        }

        public void Add(Value item)
        {
            _dbContext.Values.Add(item);
        }

        public void Delete(int id)
        {
            Value foodItem = GetSingle(id);
            _dbContext.Values.Remove(foodItem);
        }

        public Value Update(int id, Value item)
        {
            _dbContext.Values.Update(item);
            return item;
        }

        public IQueryable<Value> GetAll()
        {
            return _dbContext.Values;
        }

        public int Count()
        {
            return _dbContext.Values.Count();
        }

        public bool Save()
        {
            return (_dbContext.SaveChanges() >= 0);
        }
    }
}