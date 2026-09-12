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
    }
}
