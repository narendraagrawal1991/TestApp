using Microsoft.EntityFrameworkCore;
using OfficeApp.Models;

namespace OfficeApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<VehicleOwner> VehicleOwners { get; set; }
        public DbSet<DocumentUpload> DocumentUploads { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<LRForm> LRForms { get; set; }
        public DbSet<ItemEntry> ItemEntrys { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<LoginEntity> LoginEntitys { get; set; }
        public DbSet<LRDocumentUpload> LRDocumentUploads { get; set; }
        public DbSet<ItemCharges> ItemCharges { get; set; }
    }
}
