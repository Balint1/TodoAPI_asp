using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {

        }

        public TodoContext()
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Todo>().HasQueryFilter(p => !p.Deleted && !p.Archived);
            modelBuilder.Entity<Todo>()
                .Property(t => t.Archived)
                .HasDefaultValue(false);
            modelBuilder.Entity<Todo>()
               .Property(t => t.Deleted)
               .HasDefaultValue(false);
            modelBuilder.Entity<Todo>()
               .Property(t => t.CreationDate)
               .HasDefaultValueSql("GETDATE()");
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Todo> Todos { get; set; }

        public DbSet<TodoCategory> Types { get; set; }

    }
}
