using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;

namespace GiftOfTheGivers.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: shows the empty form
        public IActionResult Create()
        {
            return View();
        }

        // POST: handles form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Volunteer volunteer)
        {
            if (!ModelState.IsValid)
            {
                return View(volunteer);
            }

            // Leave volunteer.UserId as null unless the application explicitly sets it elsewhere
            _context.Volunteers.Add(volunteer);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thank you for signing up as a volunteer.";
            return RedirectToAction(nameof(Create));
        }

        // GET: Employee-only list of sign-ups
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Index()
        {
            var volunteers = await _context.Volunteers.ToListAsync();
            return View(volunteers);
        }
    }
}
