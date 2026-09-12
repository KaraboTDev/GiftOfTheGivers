using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class ReliefProjects
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
        public DisasterType ProjectDisasterType { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CurrentStatus Status { get; set; }

        [Precision(18, 2)]
        public decimal FundingTarget { get; set; }

        [Precision(18, 2)]
        public decimal FundsRaised { get; set; }

        public int NumberOfBeneficiaries { get; set; }
        public string ProjectManagerUserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
