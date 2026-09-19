using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace GiftOfTheGivers.Models
{
    public class Volunteer
    {
        public int VolunteerId { get; set; }
        public string? UserId { get; set; } // nullable — form doesn't require login
        public string Skills { get; set; } = string.Empty;
        public string Availability { get; set; } = string.Empty;
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;
        public string MedicalConditions { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public DateTime? LastDeployedDate { get; set; }
        public int MaxTravelDistance { get; set; }
        public string LanguagesSpoken { get; set; } = string.Empty;

        // Every availability period this volunteer has added
        public ICollection<VolunteerAvailability> AvailabilityPeriods { get; set; } = new List<VolunteerAvailability>();
        
        // Every skill this volunteer has selected
        public ICollection<VolunteerSkill> VolunteerSkills { get; set; } = new List<VolunteerSkill>();
    }
}

