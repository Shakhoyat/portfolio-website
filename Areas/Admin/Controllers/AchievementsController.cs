#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Services;
using PortfolioWebsite.Models;

namespace PortfolioWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AchievementsController : Controller
    {
        private readonly IPortfolioService _portfolioService;

        public AchievementsController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public async Task<IActionResult> Index()
        {
            var achievements = await _portfolioService.GetAchievementsAsync();
            return View(achievements);
        }

        public IActionResult Create()
        {
            return View(new Achievement());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Achievement achievement)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    achievement.CreatedDate = DateTime.UtcNow;
                    await _portfolioService.CreateAchievementAsync(achievement);
                    TempData["Success"] = "Achievement created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error creating achievement: " + ex.Message;
                }
            }
            return View(achievement);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var achievement = await _portfolioService.GetAchievementByIdAsync(id);
            if (achievement == null)
            {
                return NotFound();
            }
            return View(achievement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Achievement achievement)
        {
            if (id != achievement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _portfolioService.UpdateAchievementAsync(achievement);
                    TempData["Success"] = "Achievement updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error updating achievement: " + ex.Message;
                }
            }
            return View(achievement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _portfolioService.DeleteAchievementAsync(id);
                TempData["Success"] = "Achievement deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting achievement: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
