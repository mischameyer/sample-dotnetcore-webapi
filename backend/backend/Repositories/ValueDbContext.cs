using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class ValueDbContext : DbContext
    {
        public ValueDbContext(DbContextOptions<ValueDbContext> options) : base(options)
        {
        }

        public DbSet<Value> Values { get; set; }

    }
}