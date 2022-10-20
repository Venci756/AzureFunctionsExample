using Microsoft.EntityFrameworkCore;
using TodoFuncs.Models;

namespace TodoFuncs.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Todo>(entity => entity.HasKey(x => x.Id));
        }
    }
}