using DemoCoreApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoCoreApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<GuestResponse> Responses { get; set; }
    }
}
