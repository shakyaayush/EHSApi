using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EHSApi.Models
{
    public partial class EHSDbContext : DbContext
    {
        public EHSDbContext()
        {
        }

        public EHSDbContext(DbContextOptions<EHSDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Buyer> Buyers { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<Seller> Sellers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=EHSDb;Trusted_Connection=True;");
            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Buyer>(entity =>
            {
                entity.Property(e => e.DateofBirth).HasColumnType("date");

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastName).HasMaxLength(255);

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.BuyerId)
                    .HasConstraintName("FK__Cart__BuyerId__2D27B809");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.PropertyId)
                    .HasConstraintName("FK__Cart__PropertyId__2E1BDC42");
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.InitialDeposit).HasColumnType("money");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Landmark)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PriceRange).HasColumnType("money");

                entity.Property(e => e.PropertyName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PropertyOption)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PropertyType)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__Propertie__Selle__2A4B4B5E");
            });

            modelBuilder.Entity<Seller>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.DateofBirth).HasColumnType("date");

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastName).HasMaxLength(255);

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
