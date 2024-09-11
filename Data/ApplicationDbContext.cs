using astAttempt.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace astAttempt.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        public DbSet<CargoOrder> CargoOrders { get; set; }

        public DbSet<CargoOrderDetails> CargoOrderDetails { get; set; }

        public DbSet<City> Citys { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<UserMaster> UserMasters { get; set; }


    }
}
