
using Domains.Entities;
using Domains.Identity;
using Domains.Views;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DAL.ApplicationContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<TbCategory> TbCategory { get; set; } = null!;
        public DbSet<TbAuthor> TbAuthor { get; set; } = null!;
        public DbSet<TbBook> TbBook { get; set; } = null!;
        public DbSet<TbFavoriteBook> TbFavoriteBook { get; set; } = null!;
        public DbSet<TbReport> TbReport { get; set; } = null!;
        public DbSet<TbReview> TbReview { get; set; } = null!;
        public DbSet<TbSettings> TbSettings { get; set; } = null!;
        public DbSet<VwBook> VwBook { get; set; } = null!;
        public DbSet<VwUserProfile> VwUserProfile { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<VwBook>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwBook");
            });

            modelBuilder.Entity<VwUserProfile>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwUserProfile");
            });

            modelBuilder.Entity<TbRefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Token)
                      .IsRequired();

                entity.Property(e => e.ExpiresAt)
                      .IsRequired();

                entity.Property(e => e.CurrentState)
                      .HasDefaultValue(1);


            });

        }
    }

}
