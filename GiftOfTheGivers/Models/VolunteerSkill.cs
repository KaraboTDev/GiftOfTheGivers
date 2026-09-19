namespace GiftOfTheGivers.Models
{
    public class VolunteerSkill
    {
        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;

        public int SkillId { get; set; }
        public Skill Skill { get; set; } = null!;
    }
}

