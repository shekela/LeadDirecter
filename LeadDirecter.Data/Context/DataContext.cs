using LeadDirecter.Data.Entities;
using LeadDirecter.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LeadDirecter.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Campaign> Campaigns => Set<Campaign>();
        public DbSet<DestinationCrmConfiguration> DestinationCrmConfigurations => Set<DestinationCrmConfiguration>();
        public DbSet<Lead> Leads => Set<Lead>();
        public DbSet<CampaignValidation> CampaignValidations => Set<CampaignValidation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            // --- Campaign → DestinationCrmConfiguration (1:N)
            modelBuilder.Entity<Campaign>()
                .HasMany(c => c.DestinationCrmConfigurations)
                .WithOne(cfg => cfg.Campaign)
                .OnDelete(DeleteBehavior.Cascade);
            // If you delete a campaign → all its configs are deleted (OK)
            // But deleting a config does NOT delete the campaign

            // --- Campaign → Lead (1:N)
            modelBuilder.Entity<Campaign>()
                .HasMany(c => c.Leads)
                .WithOne()
                .HasForeignKey(l => l.CampaignId)
                .OnDelete(DeleteBehavior.Restrict);
            // Campaign can have many leads; lead belongs to one campaign
            // If you delete a campaign that still has leads, it will throw unless you remove the leads first

            modelBuilder.Entity<Campaign>()
                .HasMany(c => c.CampaignValidations)
                .WithOne(cv => cv.Campaign)
                .HasForeignKey(cv => cv.CampaignId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CampaignValidation>()
                .HasIndex(cv => new { cv.CampaignId, cv.ValidationType })
                .IsUnique();

            // --- DestinationCrmConfiguration → Lead (1:N)
            modelBuilder.Entity<DestinationCrmConfiguration>()
                .HasMany(cfg => cfg.Leads)
                .WithOne()
                .HasForeignKey(l => l.DestinationCrmConfigurationId)
                .OnDelete(DeleteBehavior.Restrict);
            // One CRM config can have many leads; each lead belongs to one config
            // Deleting a CRM config will not auto-delete leads
            modelBuilder.Entity<DestinationCrmConfiguration>()
                 .Property(c => c.LeadRegistrationContentType)
                 .HasConversion(
                     v => EnumHelper.ConvertContentTypeToString(v),
                     v => EnumHelper.ConvertStringToContentType(v)
                 );

            modelBuilder.Entity<DestinationCrmConfiguration>()
                .Property(c => c.LeadRetrievalContentType)
                .HasConversion(
                    v => EnumHelper.ConvertContentTypeToString(v),
                    v => EnumHelper.ConvertStringToContentType(v)
                );

            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.General);

            modelBuilder.Entity<DestinationCrmConfiguration>(entity =>
            {
                entity.Property(e => e.LeadRegistrationHeaders)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, jsonOptions),
                          v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, jsonOptions))
                      .HasColumnType("jsonb");

                entity.Property(e => e.LeadRegistrationBodyTemplate)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, jsonOptions),
                          v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, jsonOptions))
                      .HasColumnType("jsonb");

                entity.Property(e => e.LeadsRegistrationQueryParams)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, jsonOptions),
                          v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, jsonOptions))
                      .HasColumnType("jsonb");

                entity.Property(e => e.LeadsRetrievalHeaders)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, jsonOptions),
                          v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, jsonOptions))
                      .HasColumnType("jsonb");

                entity.Property(e => e.LeadRetrievalBodyTemplate)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, jsonOptions),
                          v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, jsonOptions))
                      .HasColumnType("jsonb");

                entity.Property(e => e.LeadsRetrievalQueryParams)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, jsonOptions),
                          v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, jsonOptions))
                      .HasColumnType("jsonb");

                entity.Property(e => e.AuthConfig)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, jsonOptions),
                          v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, jsonOptions))
                      .HasColumnType("jsonb");

                entity.Property(e => e.ErrorIdentifier)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, jsonOptions),
                          v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, jsonOptions))
                      .HasColumnType("jsonb");
            });

            modelBuilder.Entity<Lead>(entity =>
            {
                entity.Property(e => e.CampaignMacros)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, jsonOptions),
                          v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, jsonOptions))
                      .HasColumnType("jsonb");

                entity.Property(e => e.CustomProperties)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, jsonOptions),
                          v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, jsonOptions))
                      .HasColumnType("jsonb");
            });
        }
    }
}
