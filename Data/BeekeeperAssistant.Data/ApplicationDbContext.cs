﻿namespace BeekeeperAssistant.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Models;
    using BeekeeperAssistant.Data.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Apiary> Apiaries { get; set; }

        public DbSet<Beehive> Beehives { get; set; }

        public DbSet<Inspection> Inspections { get; set; }

        public DbSet<Treatment> Treatments { get; set; }

        public DbSet<TreatedBeehive> TreatedBeehives { get; set; }

        public DbSet<Harvest> Harvests { get; set; }

        public DbSet<HarvestedBeehive> HarvestedBeehives { get; set; }

        public DbSet<Duty> Duties { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Queen> Queens { get; set; }

        public DbSet<ApiaryHelper> ApiaryHelpers { get; set; }

        public DbSet<BeehiveHelper> BeehivesHelpers { get; set; }

        public DbSet<QueenHelper> QueensHelpers { get; set; }

        public DbSet<BeehiveNote> BeehiveNotes { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Beehive>()
                .HasOne(b => b.Queen)
                .WithOne(q => q.Beehive)
                .HasForeignKey<Queen>(q => q.BeehiveId);

            builder.Entity<Beehive>()
                .HasOne(b => b.Owner)
                .WithMany(o => o.Beehives)
                .HasForeignKey(b => b.OwnerId);

            builder.Entity<Inspection>()
                .HasOne(i => i.Owner)
                .WithMany(o => o.Inspections)
                .HasForeignKey(i => i.OwnerId);

            builder.Entity<Treatment>()
                .HasOne(t => t.Owner)
                .WithMany(o => o.Treatments)
                .HasForeignKey(t => t.OwnerId);

            builder.Entity<Harvest>()
                .HasOne(h => h.Owner)
                .WithMany(o => o.Harvests)
                .HasForeignKey(h => h.OwnerId);

            builder.Entity<Queen>()
                .HasOne(q => q.Owner)
                .WithMany(o => o.Queens)
                .HasForeignKey(q => q.OwnerId);

            builder.Entity<TreatedBeehive>()
                .HasKey(tb => new { tb.BeehiveId, tb.TreatmentId });

            builder.Entity<TreatedBeehive>()
                .HasOne<Beehive>(b => b.Beehive)
                .WithMany(b => b.TreatedBeehives)
                .HasForeignKey(b => b.BeehiveId);

            builder.Entity<TreatedBeehive>()
                .HasOne<Treatment>(t => t.Treatment)
                .WithMany(t => t.TreatedBeehives)
                .HasForeignKey(t => t.TreatmentId);

            builder.Entity<HarvestedBeehive>()
                .HasKey(hb => new { hb.BeehiveId, hb.HarvestId });

            builder.Entity<HarvestedBeehive>()
                .HasOne<Beehive>(b => b.Beehive)
                .WithMany(b => b.HarvestedBeehives)
                .HasForeignKey(b => b.BeehiveId);

            builder.Entity<HarvestedBeehive>()
                .HasOne<Harvest>(h => h.Harvest)
                .WithMany(h => h.HarvestedBeehives)
                .HasForeignKey(h => h.HarvestId);

            builder.Entity<ApiaryHelper>()
                .HasKey(ah => new { ah.ApiaryId, ah.UserId });

            builder.Entity<BeehiveHelper>()
                .HasKey(ah => new { ah.BeehiveId, ah.UserId });

            builder.Entity<QueenHelper>()
                .HasKey(ah => new { ah.QueenId, ah.UserId });

            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            this.ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Applies configurations
        private void ConfigureUserIdentityRelations(ModelBuilder builder)
             => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
