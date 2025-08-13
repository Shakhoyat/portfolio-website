#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Services;
using PortfolioWebsite.Models;

namespace PortfolioWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PersonalInfoController : Controller
    {
        private readonly IPortfolioService _portfolioService;

        public PersonalInfoController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public async Task<IActionResult> Index()
        {
            var personalInfo = await _portfolioService.GetPersonalInfoAsync();
            return View(personalInfo);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var personalInfo = await _portfolioService.GetPersonalInfoAsync();
            if (personalInfo == null)
            {
                personalInfo = new PersonalInfo
                {
                    Id = 1,
                    Name = "",
                    Title = "",
                    Subtitle = "",
                    Bio = "",
                    Location = "",
                    Email = "",
                    Phone = "",
                    ResumeUrl = "",
                    ProfileImageUrl = "",
                    IsActive = true
                };
            }
            return View(personalInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PersonalInfo personalInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _portfolioService.UpdatePersonalInfoAsync(personalInfo);
                    TempData["Success"] = "Personal information updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error updating personal information: " + ex.Message;
                }
            }
            return View(personalInfo);
        }
    }
}
