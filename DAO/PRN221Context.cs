using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace BusinessObject.Models
{
    public partial class PRN221_ProjectContext : DbContext
    {
        public PRN221_ProjectContext()
        {
        }

        public PRN221_ProjectContext(DbContextOptions<PRN221_ProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Lot> Lots { get; set; } = null!;
        public virtual DbSet<LotDetail> LotDetails { get; set; } = null!;
        public virtual DbSet<Partner> Partners { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<StockOut> StockOuts { get; set; } = null!;
        public virtual DbSet<StockOutDetail> StockOutDetails { get; set; } = null!;
        public virtual DbSet<StorageArea> StorageAreas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var strConn = config["ConnectionStrings:DefaultConnection"];
            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.AccountCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Lot>(entity =>
            {
                entity.ToTable("Lot");

                entity.Property(e => e.DateIn).HasColumnType("date");

                entity.Property(e => e.LotCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Note).HasColumnType("text");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Lots)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lot__AccountId__3F466844");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.Lots)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lot__PartnerId__403A8C7D");
            });

            modelBuilder.Entity<LotDetail>(entity =>
            {
                entity.ToTable("LotDetail");

                entity.HasOne(d => d.Lot)
                    .WithMany(p => p.LotDetails)
                    .HasForeignKey(d => d.LotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LotDetail__LotId__4E88ABD4");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.LotDetails)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LotDetail__Partn__5070F446");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.LotDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LotDetail__Produ__4F7CD00D");
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("Partner");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PartnerCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.AreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__AreaId__47DBAE45");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__Categor__46E78A0C");
            });

            modelBuilder.Entity<StockOut>(entity =>
            {
                entity.ToTable("StockOut");

                entity.Property(e => e.DateOut).HasColumnType("date");

                entity.Property(e => e.Note).HasColumnType("text");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.StockOuts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StockOut__Accoun__4316F928");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.StockOuts)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StockOut__Partne__440B1D61");
            });

            modelBuilder.Entity<StockOutDetail>(entity =>
            {
                entity.ToTable("StockOutDetail");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.StockOutDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StockOutD__Produ__4AB81AF0");

                entity.HasOne(d => d.StockOut)
                    .WithMany(p => p.StockOutDetails)
                    .HasForeignKey(d => d.StockOutId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StockOutD__Stock__4BAC3F29");
            });

            modelBuilder.Entity<StorageArea>(entity =>
            {
                entity.HasKey(e => e.AreaId)
                    .HasName("PK__StorageA__70B82048EEAA8ABF");

                entity.ToTable("StorageArea");

                entity.Property(e => e.AreaCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AreaName).HasMaxLength(100);
            });
            modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
            .HasMany(p => p.StockOutDetails)
            .WithOne(sod => sod.Product)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Product>()
            .HasMany(p => p.LotDetails)
            .WithOne(ld => ld.Product)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Lot>()
            .HasOne(l => l.Partner)
            .WithMany(p => p.Lots)
        .HasForeignKey(l => l.PartnerId)
        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Product>()
    .HasMany(p => p.StockOutDetails)
    .WithOne(sod => sod.Product)
    .OnDelete(DeleteBehavior.Cascade);



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
