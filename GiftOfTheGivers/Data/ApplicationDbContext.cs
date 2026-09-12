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


        }
    }

}
