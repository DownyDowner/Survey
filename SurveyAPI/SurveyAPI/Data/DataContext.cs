using Microsoft.EntityFrameworkCore;
using SurveyAPI.Entities;

namespace SurveyAPI.Data {
    public class DataContext : DbContext {
        public DataContext(DbContextOptions<DataContext> options): base(options) {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuestionEntity>(builder => {
                builder.HasKey(e => e.Id);
                builder.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ChoiceEntity>(builder => {
                builder.HasKey(e => e.Id);
                builder.Property(e => e.Id).ValueGeneratedOnAdd();

                builder.HasOne(e => e.Question)
                    .WithMany(e => e.Choices)
                    .HasForeignKey(e => e.IdQuestion);
            });
        }

        public DbSet<QuestionEntity> Questions { get; set; }
        public DbSet<ChoiceEntity> Choices { get; set; }
    }
}
