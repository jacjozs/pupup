using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PupUp.Models;
using PupUp.Models.Badges;
using PupUp.Models.Dogs;
using PupUp.Models.Identity;
using PupUp.Models.Quests;
using PupUp.Models.Trainings;

namespace PupUp.Data
{
    public class PupUpDbContext : IdentityDbContext<PupUpUser>
    {
        public DbSet<Dog> Dogs { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingStep> TrainingSteps { get; set; }
        public DbSet<DogTrainingState> DogTrainingStates { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<UserQuest> UserQuests { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<Points> Points { get; set; }
        public PupUpDbContext(DbContextOptions<PupUpDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PupUpUser>().HasMany(d => d.Dogs).WithOne(p => p.User).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Training>().HasMany(t => t.Steps).WithOne(s => s.Training).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DogTrainingState>().HasKey(x => new { x.DogId, x.TrainingId });
            builder.Entity<DogTrainingState>().HasOne(s => s.Training).WithOne();
            builder.Entity<DogTrainingState>().HasOne(s => s.Dog).WithOne();

            builder.Entity<Dog>().HasMany(d => d.TrainingStates).WithOne(s => s.Dog).HasForeignKey(d => d.DogId).HasPrincipalKey(d => d.Id).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Training>().HasMany(d => d.TrainingStates).WithOne(s => s.Training).HasForeignKey(d => d.TrainingId).HasPrincipalKey(d => d.Id).OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<UserQuest>().HasKey(x => new { x.QuestId, x.UserId });
            builder.Entity<UserQuest>().HasOne(s => s.Quest).WithMany();
            builder.Entity<UserQuest>().HasOne(s => s.User).WithMany();

            builder.Entity<UserBadge>().HasKey(x => new { x.BadgeId, x.UserId });
            builder.Entity<UserBadge>().HasOne(s => s.Badge).WithMany();
            builder.Entity<UserBadge>().HasOne(s => s.User).WithMany();

            builder.Entity<Points>().HasOne(s => s.User).WithOne(u => u.Points);


            builder.Entity<IdentityRole>(entity =>
            {
                entity.HasData(new IdentityRole
                {
                    Id = "cde51a07-6f5f-4ee6-8f15-5e33eda3b608",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "d5ac2162-b476-4114-b3ec-482c3c14f6ab"
                });
                entity.HasData(new IdentityRole
                {
                    Id = "337bb967-d41f-4757-9a25-6dac33bbdfa5",
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "62ea38ee-e763-4ae6-b9bc-589644173311"
                });

            });
        }
    }
}

