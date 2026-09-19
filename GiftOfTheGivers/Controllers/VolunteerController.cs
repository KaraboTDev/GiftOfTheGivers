using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using System.Collections.Generic;
using System.Linq;

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
        public async Task<IActionResult> Create()
        {
            ViewBag.AllSkills = await _context.Skills.OrderBy(s => s.Name).ToListAsync();
            return View();
        }

        // SelectedSkillIds (skills multi-select)
        // AvailabilityStart/End/Note (availability calendar)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Volunteer volunteer,
            List<int> SelectedSkillIds,
            List<DateTime> AvailabilityStart,
            List<DateTime> AvailabilityEnd,
            List<string?> AvailabilityNote)
        {
            // Skills and Availability are no longer filled in by direct text inputs on the form 
            // they're now populated from SelectedSkillIds and the AvailabilityStart/End/Note lists
            ModelState.Remove(nameof(Volunteer.Skills));
            ModelState.Remove(nameof(Volunteer.Availability));

            if (!ModelState.IsValid)
            {
                ViewBag.AllSkills = await _context.Skills.OrderBy(s => s.Name).ToListAsync();
                return View(volunteer);
            }

            if (AvailabilityStart != null) //Turn each submitted date range into a VolunteerAvailability record
            {
                for (int i = 0; i < AvailabilityStart.Count; i++)
                {
                    volunteer.AvailabilityPeriods.Add(new VolunteerAvailability
                    {
                        StartDate = AvailabilityStart[i],
                        EndDate = AvailabilityEnd[i],
                        Note = AvailabilityNote != null && i < AvailabilityNote.Count ? AvailabilityNote[i] : null
                    });
                }
            }

            if (SelectedSkillIds != null) //link every skill the volunteer selected to this volunteer
            {
                foreach (var skillId in SelectedSkillIds.Distinct())
                {
                    volunteer.VolunteerSkills.Add(new VolunteerSkill { SkillId = skillId });
                }
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
