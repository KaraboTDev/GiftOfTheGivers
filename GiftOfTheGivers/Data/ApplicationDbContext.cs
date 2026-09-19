using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<VolunteerAssignments> VolunteerAssignments { get; set; }
        public DbSet<Donations> Donations { get; set; }
        public DbSet<ReliefProjects> ReliefProjects { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<VolunteerAvailability> VolunteerAvailabilities { get; set; }
        public DbSet<VolunteerSkill> VolunteerSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VolunteerAssignments>()
                        .HasIndex(x => new { x.VolunteerId, x.ProjectId })
                        .IsUnique();
                       

            //Donations can be anonymous, so DonorUserId is optional
            //A donation should still be valid even with no linked user account.
            modelBuilder.Entity<Donations>()
                        .HasOne<ApplicationUser>()
                        .WithMany()
                        .HasForeignKey(d => d.DonationUserId)
                        .IsRequired(false);

            modelBuilder.Entity<Donations>()
                        .HasOne<ReliefProjects>()
                        .WithMany()
                        .HasForeignKey(d => d.ProjectId)
                        .IsRequired(false);

            modelBuilder.Entity<Volunteer>()
                        .HasOne<ApplicationUser>()
                        .WithMany()
                        .HasForeignKey(v => v.UserId)
                        .IsRequired(false); // volunteer registration doesn't require an account

            modelBuilder.Entity<ReliefProjects>()
                        .HasOne<ApplicationUser>()
                        .WithMany()
                        .HasForeignKey(p => p.ProjectManagerUserId)
                        .IsRequired(false); // Project manager can be null

            modelBuilder.Entity<VolunteerAssignments>()
                        .HasOne<Volunteer>()
                        .WithMany()
                        .HasForeignKey(va => va.VolunteerId);

            modelBuilder.Entity<VolunteerAssignments>()
                       .HasOne<ReliefProjects>()
                       .WithMany()
                       .HasForeignKey(va => va.ProjectId);

            modelBuilder.Entity<VolunteerSkill>()
    .HasKey(vs => new { vs.VolunteerId, vs.SkillId });

            modelBuilder.Entity<VolunteerSkill>()
                .HasOne(vs => vs.Volunteer)
                .WithMany(v => v.VolunteerSkills)
                .HasForeignKey(vs => vs.VolunteerId);

            modelBuilder.Entity<VolunteerSkill>()
                .HasOne(vs => vs.Skill)
                .WithMany(s => s.VolunteerSkills)
                .HasForeignKey(vs => vs.SkillId);

            modelBuilder.Entity<VolunteerAvailability>()
                .HasOne(a => a.Volunteer)
                .WithMany(v => v.AvailabilityPeriods)
                .HasForeignKey(a => a.VolunteerId);

            modelBuilder.Entity<Skill>().HasData(
                new Skill { Id = 1, Name = "First Aid" },
                new Skill { Id = 2, Name = "Search & Rescue" },
                new Skill { Id = 3, Name = "Medical" },
                new Skill { Id = 4, Name = "Driving" },
                new Skill { Id = 5, Name = "Cooking" },
                new Skill { Id = 6, Name = "Logistics" }
            );


        }
    }

}
