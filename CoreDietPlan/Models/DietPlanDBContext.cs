using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoreDietPlan.Models
{
    public partial class DietPlanDBContext : DbContext
    {
        public DietPlanDBContext()
        {
        }

        public DietPlanDBContext(DbContextOptions<DietPlanDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminsTable> AdminsTable { get; set; }
        public virtual DbSet<Allergies> Allergies { get; set; }
        public virtual DbSet<Combinations> Combinations { get; set; }
        public virtual DbSet<DailyActivityTable> DailyActivityTable { get; set; }
        public virtual DbSet<DietDb> DietDb { get; set; }
        public virtual DbSet<DietUsers> DietUsers { get; set; }
        public virtual DbSet<ExercisesListTable> ExercisesListTable { get; set; }
        public virtual DbSet<FoodData> FoodData { get; set; }
        public virtual DbSet<FoodDb> FoodDb { get; set; }
        public virtual DbSet<NutritionalValue> NutritionalValue { get; set; }
        public virtual DbSet<Preferences> Preferences { get; set; }
        public virtual DbSet<ProgressTracker> ProgressTracker { get; set; }
        public virtual DbSet<UserDailyConsumption> UserDailyConsumption { get; set; }
        public virtual DbSet<UserMacros> UserMacros { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:dietplanner.database.windows.net,1433;Initial Catalog=DietPlanDB;Persist Security Info=False;User ID=Adrien;Password=dietplanner123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<AdminsTable>(entity =>
            {
                entity.HasKey(e => e.AdminUsername);

                entity.Property(e => e.AdminUsername)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AdminName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AdminPassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Allergies>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.Property(e => e.RecordId).ValueGeneratedNever();

                entity.Property(e => e.AllergiesList)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Combinations>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.Property(e => e.RecordId)
                    .HasColumnName("RecordID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Combinations1)
                    .IsRequired()
                    .HasColumnName("Combinations")
                    .IsUnicode(false);

                entity.Property(e => e.MealTime)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Preferences)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DailyActivityTable>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.Property(e => e.RecordId).ValueGeneratedNever();

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.Property(e => e.ActivityName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Duration)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RecordedDate).HasColumnType("date");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DietDb>(entity =>
            {
                entity.HasKey(e => e.FoodId)
                    .HasName("PK__DietDB__856DB3CBFB3F7D16");

                entity.ToTable("DietDB");

                entity.Property(e => e.FoodId).HasColumnName("FoodID");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.MealTime)
                    .HasColumnName("mealTime")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Meals)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Serving)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DietUsers>(entity =>
            {
                entity.HasKey(e => e.UserName);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.IsVeg).HasColumnName("isVeg");

                entity.Property(e => e.ResetPasswordCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserFirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserGender)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserLastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserWeightCategory)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ExercisesListTable>(entity =>
            {
                entity.HasKey(e => e.ExerciseId);

                entity.Property(e => e.ExerciseId).ValueGeneratedNever();

                entity.Property(e => e.ExerciseList)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FoodData>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Food)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FoodImg)
                    .HasColumnName("Food_img")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Serving)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FoodDb>(entity =>
            {
                entity.ToTable("FoodDB");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Fcategory)
                    .HasColumnName("FCategory")
                    .IsUnicode(false);

                entity.Property(e => e.Fgroup)
                    .HasColumnName("FGroup")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Food)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MealTime).IsUnicode(false);

                entity.Property(e => e.Serving)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NutritionalValue>(entity =>
            {
                entity.HasKey(e => e.FoodId)
                    .HasName("PK__Nutritio__856DB3CBB1AA8DE4");

                entity.Property(e => e.FoodId).HasColumnName("FoodID");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Food)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FoodImg)
                    .HasColumnName("Food_img")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MealTime)
                    .HasColumnName("mealTime")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TotalCalorie).HasColumnName("Total_Calorie");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Preferences>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.Property(e => e.RecordId).ValueGeneratedNever();

                entity.Property(e => e.PreferencesList).IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProgressTracker>(entity =>
            {
                entity.HasKey(e => e.TrackerId);

                entity.Property(e => e.TrackerId).ValueGeneratedNever();

                entity.Property(e => e.ActivityLevel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateEntered).HasColumnType("date");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserDailyConsumption>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.Property(e => e.RecordId)
                    .HasColumnName("RecordID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ConsumptionId).HasColumnName("ConsumptionID");

                entity.Property(e => e.RecordedDate).HasColumnType("date");

                entity.Property(e => e.Username).IsUnicode(false);
            });

            modelBuilder.Entity<UserMacros>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("PK_UserMacros_1");

                entity.Property(e => e.RecordId).HasColumnName("recordID");

                entity.Property(e => e.Bmi)
                    .IsRequired()
                    .HasColumnName("BMI")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RecordedDate).HasColumnType("date");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });
        }
    }
}
