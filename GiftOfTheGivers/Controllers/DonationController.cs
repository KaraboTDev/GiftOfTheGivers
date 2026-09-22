using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using System.Net.Http.Json;

namespace GiftOfTheGivers.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Donations donation)
        {
            if (!ModelState.IsValid)
            {
                return View(donation);
            }

            // If donor chose not to be anonymous and is authenticated, attach their user id
            if (!donation.IsAnonymous && User.Identity != null && User.Identity.IsAuthenticated)
            {
                donation.DonationUserId = _userManager.GetUserId(User);
            }
            else
            {
                donation.DonationUserId = null;
            }

            donation.DonationDate = DateTime.Now;
            donation.TaxCertGenDate = DateTime.Now; // generated instantly, per earlier decision

            // TODO 2: add donation to _context, save
            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            // TODO 3: set TaxCertNum to the DB-generated DonationId and save again
            donation.TaxCertNum = donation.DonationId;
            await _context.SaveChangesAsync();

            // --- Azure Function call: generate a tax certificate for this donation ---
            try
            {
                // Work out a display name for the certificate.
                // If anonymous (or not logged in), just use "Anonymous Donor".
                // Otherwise, look up the logged-in user's name.
                string donorDisplayName = "Anonymous Donor";
                if (!donation.IsAnonymous && User.Identity != null && User.Identity.IsAuthenticated)
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    if (currentUser != null)
                    {
                        donorDisplayName = currentUser.UserName ?? currentUser.Email ?? "Anonymous Donor";
                    }
                }

                using var httpClient = new HttpClient();
                var functionPayload = new
                {
                    DonorName = donorDisplayName,
                    Amount = donation.DonationAmount
                };

                var functionResponse = await httpClient.PostAsJsonAsync(
                    "http://localhost:7124/api/GenerateTaxCertificate", // local Function URL while testing
                    functionPayload);

                if (functionResponse.IsSuccessStatusCode)
                {
                    var certificateJson = await functionResponse.Content.ReadAsStringAsync();
                    TempData["TaxCertificateResult"] = certificateJson;
                }
            }
            catch (Exception)
            {
                // If the Function isn't running locally, don't crash the donation flow -
                // just skip the certificate generation for now.
            }
            // --- end Azure Function call ---

            // TODO 4: redirect to Confirmation action passing id
            return RedirectToAction(nameof(Confirmation), new { id = donation.DonationId });
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            // TODO 5: fetch the single donation matching this id and pass it to the view
            var donation = await _context.Donations.FirstOrDefaultAsync(d => d.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }
    }
}
