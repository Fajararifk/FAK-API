using FAK.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FAK.Persistance
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Kabupaten> Kabupaten { get; set; }
        public DbSet<Kecamatan> Kecamatan { get; set; }
        public DbSet<Desa> Desa { get; set; }
        public DbSet<TPS> TPS { get; set; }
        public DbSet<HasilPemiluTPS> HasilPemiluTPS { get; set; }
        public DbSet<MasterSystemConfig> MasterSystemConfig { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Payment> Payments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Province>().Property(p => p.IdProvince)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Kabupaten>().Property(p => p.IdKabupaten)
               .ValueGeneratedOnAdd(); 
            modelBuilder.Entity<Kecamatan>().Property(p => p.IdKecamatan)
               .ValueGeneratedOnAdd();
            modelBuilder.Entity<Desa>().Property(p => p.IdDesa)
              .ValueGeneratedOnAdd();
            modelBuilder.Entity<TPS>().Property(p => p.IdTPS)
              .ValueGeneratedOnAdd();
            modelBuilder.Entity<HasilPemiluTPS>().Property(p => p.IdHasil)
              .ValueGeneratedOnAdd();
            modelBuilder.Entity<MasterSystemConfig>();
            modelBuilder.Entity<Patient>();
            modelBuilder.Entity<Doctor>();
            modelBuilder.Entity<Appointment>();
            modelBuilder.Entity<MedicalRecord>();
            modelBuilder.Entity<Message>();
            modelBuilder.Entity<Staff>();
            modelBuilder.Entity<Payment>();


            modelBuilder.Entity<HasilPemiluTPS>().HasKey(p => p.IdHasil);
            modelBuilder.Entity<Province>().HasKey(p => p.IdProvince);
            modelBuilder.Entity<Kabupaten>().HasKey(p => p.IdKabupaten);
            modelBuilder.Entity<Kecamatan>().HasKey(p => p.IdKecamatan);
            modelBuilder.Entity<Desa>().HasKey(p => p.IdDesa);
            modelBuilder.Entity<TPS>().HasKey(p => p.IdTPS);
        }
    }
}
