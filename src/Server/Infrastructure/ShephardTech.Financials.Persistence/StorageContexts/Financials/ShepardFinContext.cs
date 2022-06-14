using Microsoft.EntityFrameworkCore;
using ShephardTech.Financials.Entities;

namespace ShephardTech.Financials.Persistence.StorageContexts.Financials
{
    public partial class ShepardFinContext : DbContext
    {
        public ShepardFinContext(DbContextOptions<ShepardFinContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public virtual DbSet<ShepardUser> Users { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<UserRole> UserRoles { get; set; }

        public virtual DbSet<LoginToken> LoginTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShepardUser>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Email).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.PasswordSalt).IsRequired().HasMaxLength(3000);
                entity.Property(e => e.CreatedBy).IsRequired(false).HasMaxLength(30);
                entity.Property(e => e.MobilePhone).HasMaxLength(50);
                entity.Property(e => e.UpdatedBy).IsRequired(false).HasMaxLength(50);
                entity.Property(e => e.Deleted).HasDefaultValue(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
                entity.HasKey(e => e.RoleId);
                entity.Property(e => e.RoleId).IsRequired();
                entity.Property(e => e.RoleName).HasMaxLength(50)
                    .IsRequired();

            });


            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");
                entity.HasKey(e => e.UserRoleId);
                entity.Property(e => e.UserRoleId).IsRequired();
                entity.Property(e => e.UserId)
                    .IsRequired();
                entity.Property(e => e.RoleId)
                    .IsRequired();

            });

            modelBuilder.Entity<LoginToken>(entity =>
            {

                entity.HasKey(e => e.LoginTokenId);

                entity.Property(e => e.LoginTokenId).IsRequired();

                entity.Property(e => e.ExpiryDate)
                .IsRequired();

                entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(e => e.AuthToken)
                .IsRequired()
                .HasMaxLength(2000);

            });            



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
