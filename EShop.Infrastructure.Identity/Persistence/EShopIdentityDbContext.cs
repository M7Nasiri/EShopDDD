using EShop.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Identity.Persistence
{
    public sealed class EShopIdentityDbContext
    : IdentityDbContext<
        AppUser,
        IdentityRole<Guid>,
        Guid>
    {
        public EShopIdentityDbContext(
            DbContextOptions<EShopIdentityDbContext> options)
            : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(entity =>
            {
                entity.Property(x => x.FirstName)
                    .HasMaxLength(100);

                entity.Property(x => x.LastName)
                    .HasMaxLength(100);

                entity.Property(x => x.IsActive)
                    .IsRequired();
            });

            builder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshTokens");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.TokenHash)
                    .HasMaxLength(128)
                    .IsRequired();

                entity.HasIndex(x => x.TokenHash)
                    .IsUnique();

                entity.Property(x => x.CreatedByIp)
                    .HasMaxLength(64);

                entity.Property(x => x.RevokedByIp)
                    .HasMaxLength(64);

                entity.Property(x => x.ReplacedByTokenHash)
                    .HasMaxLength(128);

                entity.HasOne(x => x.User)
                    .WithMany(x => x.RefreshTokens)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
