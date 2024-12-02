using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyAPI.Entities;

namespace SurveyAPI.Data {
    public class DataContext : IdentityDbContext<IdentityUser> {
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

            modelBuilder.Entity<ResponseEntity>(builder => {
                builder.HasKey(e => e.Id);
                builder.Property(e => e.Id).ValueGeneratedOnAdd();

                builder.HasOne(e => e.Choice)
                    .WithMany(e => e.Responses)
                    .HasForeignKey(e => e.IdChoice);

                builder.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.IdUser);
            });
        }

        public DbSet<QuestionEntity> Questions { get; set; }
        public DbSet<ChoiceEntity> Choices { get; set; }
        public DbSet<ResponseEntity> Responses { get; set; }
    }
}
