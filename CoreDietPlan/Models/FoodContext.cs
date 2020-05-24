using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoreDietPlan.Models
{
    public partial class FoodContext : DbContext
    {
        public FoodContext()
        {
        }

        public FoodContext(DbContextOptions<FoodContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FoodClass> DietPlan { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer(@"Server=tcp:dietplan.database.windows.net,1433;Initial Catalog=DietPlanDB;Persist Security Info=False;User ID=Faraz;Password=dietplan123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodClass>(entity =>
            {
                entity.HasKey(e => e.User_ID);

                entity.Property(e => e.Username)
                    .IsUnicode(false);


                entity.Property(e => e.Food_ID).IsUnicode(false);

                entity.Property(e => e.Date)
                    .IsUnicode(false);

                entity.Property(e => e.Meals)
                    .IsUnicode(false);

                entity.Property(e => e.Monday)
                    .IsUnicode(false);

                entity.Property(e => e.Tuesday)
                    .IsUnicode(false);

                entity.Property(e => e.Wednesday)
                    .IsUnicode(false);

                entity.Property(e => e.Thursday)
                    .IsUnicode(false);

                entity.Property(e => e.Friday)
                    .IsUnicode(false);

                entity.Property(e => e.Saturday)
                    .IsUnicode(false);

                entity.Property(e => e.Sunday)
                    .IsUnicode(false);
            });
        }
    }
}
