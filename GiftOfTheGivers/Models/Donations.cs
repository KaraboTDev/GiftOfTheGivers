
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class Donations
    {
        [Key]
        public int DonationId { get; set; }
        public string? DonationUserId { get; set; } 
        public int? ProjectId { get; set; }

        [Precision(18, 2)]
        public decimal DonationAmount { get; set; }

        public string Currency { get; set; } = string.Empty;
        public string DonationType { get; set; } = string.Empty;
        public string RecurringFrequency { get; set; } = string.Empty;
        public DateTime DonationDate { get; set; }
        public PaymentStatus Status { get; set; }
        public bool IsAnonymous { get; set; }
        public int TaxCertNum { get; set; }
        public DateTime TaxCertGenDate { get; set; }
        public string? Notes { get; set; } 
        
    }
}
