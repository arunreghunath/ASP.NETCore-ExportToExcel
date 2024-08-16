using ExportToExcelWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ExportToExcelWebApplication.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<GuestResponse> Responses { get; set; }
    }
}
