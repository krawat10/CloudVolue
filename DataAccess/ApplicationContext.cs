using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Record> Records { get; set; }
    }
}
