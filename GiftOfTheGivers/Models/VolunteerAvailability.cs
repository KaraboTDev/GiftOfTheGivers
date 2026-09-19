namespace GiftOfTheGivers.Models
{
    public class VolunteerAvailability
    {
        public int Id { get; set; }

        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Note { get; set; }
    }
}

