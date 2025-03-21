using FILog.Models;
using Microsoft.EntityFrameworkCore;

namespace FILog.Data
{
    public class OpsProdDbContext : DbContext
    {
        public OpsProdDbContext(DbContextOptions<OpsProdDbContext> options) : base(options)
        {
            try
            {
                Database.CanConnect(); // Memeriksa koneksi ke database
                Console.WriteLine("Database connection successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection failed: {ex.Message}");
                throw;
            }
        }
        public DbSet<eLOGMaterialMasterModel> MaterialMaster { get; set; } = null!;
    }
}
