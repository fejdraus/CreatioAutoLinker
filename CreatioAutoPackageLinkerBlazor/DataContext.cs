using CreatioAutoPackageLinkerBlazor.Data;
using CreatioAutoPackageLinkerBlazor.Data.DbClass;
using CreatioAutoPackageLinkerBlazor.Data.Object;
using Microsoft.EntityFrameworkCore;

namespace CreatioAutoPackageLinkerBlazor;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        SQLitePCL.Batteries_V2.Init();
    }
    
    public DbSet<PackageForType> PackageForTypes { get; set; } = null!;
    public DbSet<ProductForType> ProductForTypes { get; set; } = null!;
    public DbSet<TypeOfPackageForProduct> TypeOfPackageForProducts { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Package> Packages { get; set; } = null!;
    public DbSet<PackageHierarchy> PackageHierarchies { get; set; } = null!;
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PackageForType>()
            .HasKey(x => x.Id)
            .HasName("PrimaryKey_Id");
        modelBuilder.Entity<ProductForType>()
            .HasKey(x => x.Id)
            .HasName("PrimaryKey_Id");
        modelBuilder.Entity<TypeOfPackageForProduct>(entity =>
        {
            entity.HasKey(x => x.Id).HasName("PrimaryKey_Id");
            entity.HasOne(n => n.Package)
                .WithMany(u => u.TypeOfPackageForProducts)
                .HasForeignKey(n => n.PackageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(n => n.Product)
                .WithMany(u => u.TypeOfPackageForProducts)
                .HasForeignKey(n => n.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(x => x.Id).HasName("PrimaryKey_Id");
            entity.HasOne(n => n.ProductForType)
                .WithMany(u => u.Projects)
                .HasForeignKey(n => n.ProductForTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(x => x.Id).HasName("PrimaryKey_Id");
            entity.HasOne(n => n.Project)
                .WithMany(u => u.Package)
                .HasForeignKey(n => n.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<PackageHierarchy>(entity =>
        {
            entity.HasKey(x => x.Id).HasName("PrimaryKey_Id");
            entity.HasOne(n => n.DependOnPackage)
                .WithMany(u => u.PackageHierarchyBasePackages)
                .HasForeignKey(n => n.DependOnPackageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(n => n.BasePackage)
                .WithMany(u => u.PackageHierarchyDependOnPackages)
                .HasForeignKey(n => n.BasePackageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}