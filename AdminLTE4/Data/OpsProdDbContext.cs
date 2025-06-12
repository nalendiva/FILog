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
        public DbSet<EngineeringPortalModel> EngineeringPortal { get; set; } = null!;
        public DbSet<DMSConcessionModel> DMSConcession { get; set; } = null!;
        public DbSet<DMSProductionPermitModel> DMSProductionPermit { get; set; } = null!;
        public DbSet<eLOGQualityAlertModel> ELOGQualityAlert { get; set;} = null!;

        public DbSet<DMSMaterialMasterModel> DMSMaterialMasterModels { get; set; } = null!;
        public DbSet<DMSFILogModel> DMSFILogModel { get; set;} = null!;
        public DbSet<DMSFIKFCModel> DMSFIKFCModels { get; set; } = null!; 
        public DbSet<eLOGMaterialMasterModel> eLOGMasterialMasterModel { get; set; } = null!;
        public DbSet<DMSSRoutingModel> DMSSRoutingModels { get; set; } = null!;


    }
}
