using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class VolunteerAssignments
    {
        [Key]
        public int AssignmentId { get; set; }
        public int VolunteerId { get; set; }
        public int ProjectId { get; set; }
        public DateTime AssignedDate { get; set; }
        public string Role { get; set; } = string.Empty;
        public AssignmentStatus Status { get; set; }
        public string? Comments { get; set; }
        public double TravelDistance { get; set; }
        public double HoursWorked { get; set; }

    }
}
