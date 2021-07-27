namespace MobileBgWebScraper.Data
{
    using MobileBgWebScraper.Models;
    using MobileBgWebScraper.Models.Common;

    using System;

    using Microsoft.EntityFrameworkCore;

    public class MobileBgDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DatabaseConfig.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Advertisement>()
                        .HasIndex(a => a.RemoteId)
                        .IsUnique();

            modelBuilder.Entity<Brand>()
                        .HasMany(brand => brand.Models)
                        .WithOne(model => model.Brand)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Region>()
                        .HasMany(region => region.Towns)
                        .WithOne(town => town.Region)
                        .OnDelete(DeleteBehavior.Restrict);

            SetUniqueConstraints(modelBuilder,
                                typeof(BodyStyle),
                                typeof(Brand),
                                typeof(Color),
                                typeof(Engine),
                                typeof(EuroStandard),
                                typeof(Model),
                                typeof(Region),
                                typeof(Town),
                                typeof(Transmission));

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Advertisement> Advertisements { get; set; }

        public DbSet<BodyStyle> BodyStyles { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Engine> Engines { get; set; }

        public DbSet<EuroStandard> EuroStandards { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Model> Models { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Transmission> Transmissions { get; set; }

        private static void SetUniqueConstraints(ModelBuilder modelBuilder, params Type[] types)
        {
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(NameableAdvertisementProperty)))
                {
                    modelBuilder.Entity(type).HasIndex("Name").IsUnique();
                }

                if (type.IsSubclassOf(typeof(TypeableAdvertisementProperty)))
                {
                    modelBuilder.Entity(type).HasIndex("Type").IsUnique();
                }
            }
        }
    }
}
