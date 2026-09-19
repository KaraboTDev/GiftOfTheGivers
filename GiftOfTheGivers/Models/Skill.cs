namespace GiftOfTheGivers.Models
{
    public class Skill
    {
        // A standardised, reusable skill tag (e.g. "First Aid"),
        // Used instead of free-text so coordinators can search volunteers accurately
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<VolunteerSkill> VolunteerSkills { get; set; } = new List<VolunteerSkill>();
    }
}

