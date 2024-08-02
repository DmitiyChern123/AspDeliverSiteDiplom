using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Entities2;

public partial class DiplomContext : DbContext
{
    public DiplomContext()
    {
    }

    public DiplomContext(DbContextOptions<DiplomContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Courier> Couriers { get; set; }

    public virtual DbSet<Korzina> Korzinas { get; set; }

    public virtual DbSet<KorzinaInOrder> KorzinaInOrders { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<StatusOrder> StatusOrders { get; set; }

    public virtual DbSet<User> Users { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fullimg)
                .IsUnicode(false)
                .HasColumnName("fullimg");
            entity.Property(e => e.Img)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Courier>(entity =>
        {
            entity.ToTable("courier");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fio)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fio");
        });

        modelBuilder.Entity<Korzina>(entity =>
        {
            entity.ToTable("korzina");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.ProductTypeId).HasColumnName("productType_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Korzinas)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_korzina_product_type1");

            entity.HasOne(d => d.User).WithMany(p => p.Korzinas)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_korzina_user");
        });

        modelBuilder.Entity<KorzinaInOrder>(entity =>
        {
            entity.ToTable("korzinaInOrder");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Order).WithMany(p => p.KorzinaInOrders)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_korzinaInOrder_order");

            entity.HasOne(d => d.Product).WithMany(p => p.KorzinaInOrders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_korzinaInOrder_product_type");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adress)
                .HasColumnType("text")
                .HasColumnName("adress");
            entity.Property(e => e.CourierId).HasColumnName("courier_id");
            entity.Property(e => e.PayType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("pay_type");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Courier).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CourierId)
                .HasConstraintName("FK_order_courier");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_status_order");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_user");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.Img)
                .HasColumnType("text")
                .HasColumnName("img");
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Opis)
                .HasColumnType("text")
                .HasColumnName("opis");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK_product_category");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.ToTable("product_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductTypes)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_type_product");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.ToTable("promotion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Image)
                .HasColumnType("text")
                .HasColumnName("image");
            entity.Property(e => e.ImageText)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("imageText");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Opis)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("opis");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Type).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_promotion_product_type");
        });

        modelBuilder.Entity<StatusOrder>(entity =>
        {
            entity.ToTable("status_order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BonusPoints).HasColumnName("bonusPoints");
            entity.Property(e => e.Login)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
