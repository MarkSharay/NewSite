using Microsoft.EntityFrameworkCore;
using PRAS_Task.Data.Models;

namespace PRAS_Task.Data
{
    public class DataContext:DbContext
    {
        public DataContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<New> News { get; set; }
    }
}
