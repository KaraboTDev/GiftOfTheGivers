using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;

namespace GiftOfTheGivers.Controllers
{
    public class ReliefProjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReliefProjectController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Public — anyone can browse ongoing relief projects
        public async Task<IActionResult> Index()
        {
            var projects = await _context.ReliefProjects.ToListAsync();
            return View(projects);
        }

        // Employee-only — posting a new relief project update
        [Authorize(Roles = "Employee")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Create(ReliefProjects project)
        {
            if (!ModelState.IsValid)
            {
                return View(project);
            }

            // set ProjectManagerUserId to the current logged-in Employee's id
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                project.ProjectManagerUserId = _userManager.GetUserId(User);
            }
            else
            {
                project.ProjectManagerUserId = null;
            }

            project.CreatedAt = DateTime.Now;

            // add to _context, save, redirect to Index
            _context.ReliefProjects.Add(project);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
