using Microsoft.EntityFrameworkCore;
using PRAS_Task.Data.Models;

namespace PRAS_Task.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<New> News { get; set; }
        public DbSet<EngNew> EngNews { get; set; }
    }
}
