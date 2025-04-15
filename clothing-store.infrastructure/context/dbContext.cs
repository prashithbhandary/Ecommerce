using clothing_store.domain.models;
using Microsoft.EntityFrameworkCore;

namespace clothing_store.infrastructure.context
{
  public class dbContext : DbContext

  {
    public dbContext(DbContextOptions<dbContext> options) : base(options) { }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<CouponUsage> CouponUsages { get; set; }
    public DbSet<Review> Reviews { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<ApplicationUser>()
    .HasMany(u => u.Addresses)
    .WithOne(a => a.User)
    .HasForeignKey(a => a.UserId)
    .OnDelete(DeleteBehavior.Restrict);


      modelBuilder.Entity<ApplicationUser>()
    .HasIndex(u => u.Email)
    .IsUnique();

      // Product ↔ Brand (Many-to-One)
      modelBuilder.Entity<Product>()
          .HasOne(p => p.Brand)
          .WithMany()
          .HasForeignKey(p => p.BrandId)
          .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Product>()
        .HasIndex(p => p.Name);

      // Product ↔ Category (Many-to-One)
      modelBuilder.Entity<Product>()
          .HasOne(p => p.Category)
          .WithMany()
          .HasForeignKey(p => p.CategoryId)
          .OnDelete(DeleteBehavior.Restrict);

      // Product ↔ ProductVariant (One-to-Many)
      modelBuilder.Entity<ProductVariant>()
          .HasOne(pv => pv.Product)
          .WithMany(p => p.Variants)
          .HasForeignKey(pv => pv.ProductId)
          .OnDelete(DeleteBehavior.Cascade);

      // Product ↔ ProductImage (One-to-Many)
      modelBuilder.Entity<ProductImage>()
          .HasOne(pi => pi.Product)
          .WithMany(p => p.Images)
          .HasForeignKey(pi => pi.ProductId)
          .OnDelete(DeleteBehavior.Cascade);

      // Optional: Enforce uniqueness on SKU (if used)
      modelBuilder.Entity<ProductVariant>()
          .HasIndex(pv => pv.SKU)
          .IsUnique();

      modelBuilder.Entity<Cart>()
    .HasOne(c => c.User)
    .WithOne()
    .HasForeignKey<Cart>(c => c.UserId)
    .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<CartItem>()
          .HasOne(ci => ci.Cart)
          .WithMany(c => c.Items)
          .HasForeignKey(ci => ci.CartId)
          .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<CartItem>()
     .HasOne(ci => ci.Product)
     .WithMany()
     .HasForeignKey(ci => ci.ProductId)
     .OnDelete(DeleteBehavior.Restrict);


      modelBuilder.Entity<CartItem>()
          .HasOne(ci => ci.Variant)
          .WithMany()
          .HasForeignKey(ci => ci.ProductVariantId)
          .OnDelete(DeleteBehavior.SetNull);

      modelBuilder.Entity<Order>()
     .HasOne(o => o.User)
     .WithMany()
     .HasForeignKey(o => o.UserId)
     .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Order>()
          .HasOne(o => o.Address)
          .WithMany()
          .HasForeignKey(o => o.AddressId)
          .OnDelete(DeleteBehavior.Restrict); 

      modelBuilder.Entity<Order>()
          .HasOne(o => o.Payment)
          .WithOne(p => p.Order)
          .HasForeignKey<Payment>(p => p.OrderId)
          .OnDelete(DeleteBehavior.Cascade); 

      modelBuilder.Entity<Order>()
          .HasOne(o => o.Shipment)
          .WithOne(s => s.Order)
          .HasForeignKey<Shipment>(s => s.OrderId)
          .OnDelete(DeleteBehavior.Cascade);


      modelBuilder.Entity<OrderItem>()
    .HasOne(oi => oi.Product)
    .WithMany()
    .HasForeignKey(oi => oi.ProductId)
    .OnDelete(DeleteBehavior.Restrict); 

      modelBuilder.Entity<OrderItem>()
          .HasOne(oi => oi.Variant)
          .WithMany()
          .HasForeignKey(oi => oi.ProductVariantId)
          .OnDelete(DeleteBehavior.SetNull); 

      modelBuilder.Entity<OrderItem>()
          .HasOne(oi => oi.Order)
          .WithMany(o => o.Items)
          .HasForeignKey(oi => oi.OrderId)
          .OnDelete(DeleteBehavior.Cascade);


      modelBuilder.Entity<Coupon>()
    .HasIndex(c => c.Code)
    .IsUnique();

      modelBuilder.Entity<CouponUsage>()
          .HasOne(cu => cu.Coupon)
          .WithMany(c => c.Usages)
          .HasForeignKey(cu => cu.CouponId);

      modelBuilder.Entity<CouponUsage>()
          .HasOne(cu => cu.User)
          .WithMany()
          .HasForeignKey(cu => cu.UserId);

      modelBuilder.Entity<CouponUsage>()
          .HasOne(cu => cu.Order)
          .WithMany()
          .HasForeignKey(cu => cu.OrderId);

      modelBuilder.Entity<Review>()
          .HasOne(r => r.Product)
          .WithMany()
          .HasForeignKey(r => r.ProductId);

      modelBuilder.Entity<Review>()
          .HasOne(r => r.User)
          .WithMany()
          .HasForeignKey(r => r.UserId);

    }

  }
  }
