using cbk.image.Infrastructure.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace cbk.image.Infrastructure.Database
{
    public class DBContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<ImageInformation> ImageInformations { get; set; }
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Account>()
                .Property(a => a.Name)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .Property(a => a.Password)
                .HasMaxLength(64)
                .IsRequired();

            modelBuilder.Entity<ImageInformation>()
              .Property(a => a.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<ImageInformation>()
                .Property(i => i.AccountName)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<ImageInformation>()
                .Property(i => i.OriginalFileName)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<ImageInformation>()
                .Property(i => i.FileName)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<ImageInformation>()
                .Property(i => i.FileLinkPath)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}