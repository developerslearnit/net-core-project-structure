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

        public virtual DbSet<Todo> Todos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShepardUser>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Email).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Password).IsRequired();
            });

            modelBuilder.Entity<Todo>(ent =>
            {
                ent.HasKey(e => e.TodoId);
                ent.Property(e => e.Title).IsRequired().HasMaxLength(100);
                ent.Property(e => e.Content).IsRequired().HasMaxLength(1000);
                ent.Property(e => e.DueDate).IsRequired();
                ent.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
