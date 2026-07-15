using hello_world.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace hello_world.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<PersonEntity> Persons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PersonEntity>(entity =>
            {
                entity.ToTable("Persons");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .IsRequired()
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);
                entity.HasIndex(e => e.Id)
                    .IsUnique();
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20);
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(20);
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(7);

                entity.Property<DateTime>("CreateAt")
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property<DateTime>("UpdatedAt")
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");
                entity.Ignore(e => e.FullName);
            });
        }
    }
}
