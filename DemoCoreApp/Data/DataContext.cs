using Demo.ExportToExcel.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.ExportToExcel.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<GuestResponse> Responses { get; set; }
    }
}
