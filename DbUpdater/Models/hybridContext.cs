using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DbUpdater.Models
{
    public partial class hybridContext : DbContext
    {
        public hybridContext()
        {
            Database.EnsureCreated();
        }

        public hybridContext(DbContextOptions<hybridContext> options)
            : base(options)
        {
            
            
        }

        public virtual DbSet<GeoipBlocks> GeoipBlocks { get; set; }
        public virtual DbSet<GeoipLocations> GeoipLocations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseNpgsql("Host=localhost;Database=hybrid;Username=postgres;Password=1234567890");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeoipBlocks>(entity =>
            {
                entity.HasIndex(e => e.Network);

                entity.HasKey(e => e.GeoblockId)
                    .HasName("geoip_blocks_pkey");

                entity.ToTable("geoip_blocks");

                entity.Property(e => e.GeoblockId)
                    .HasColumnName("geoblock_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AccuracyRadius).HasColumnName("accuracy_radius");

                entity.Property(e => e.GeonameId).HasColumnName("geoname_id");

                entity.Property(e => e.IsAnonymousProxy).HasColumnName("is_anonymous_proxy");

                entity.Property(e => e.IsSatelliteProvider).HasColumnName("is_satellite_provider");

                entity.Property(e => e.Latitude).HasColumnName("latitude");

                entity.Property(e => e.Longitude).HasColumnName("longitude");

                entity.Property(e => e.Network)
                    .HasColumnName("network")
                    .HasMaxLength(100);

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postal_code")
                    .HasMaxLength(10);

                entity.Property(e => e.RegisteredCountryGeonameId).HasColumnName("registered_country_geoname_id");

                entity.Property(e => e.RepresentedCountryGeonameId).HasColumnName("represented_country_geoname_id");
            });

            modelBuilder.Entity<GeoipLocations>(entity =>
            {
                entity.HasIndex(e => e.GeonameId);

                entity.HasKey(e => e.GeolocationId)
                    .HasName("geoip_locations_pkey");

                entity.ToTable("geoip_locations");

                entity.Property(e => e.GeolocationId)
                    .HasColumnName("geolocation_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.ContinentCode)
                    .HasColumnName("continent_code")
                    .HasMaxLength(2);

                entity.Property(e => e.ContinentName)
                    .HasColumnName("continent_name")
                    .HasMaxLength(255);

                entity.Property(e => e.CountryIsoCode)
                    .HasColumnName("country_iso_code")
                    .HasMaxLength(2);

                entity.Property(e => e.CountryName)
                    .HasColumnName("country_name")
                    .HasMaxLength(255);

                entity.Property(e => e.GeonameId).HasColumnName("geoname_id");

                entity.Property(e => e.IsInEuropeanUnion).HasColumnName("is_in_european_union");

                entity.Property(e => e.LocaleCode)
                    .HasColumnName("locale_code")
                    .HasMaxLength(2);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
