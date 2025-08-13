#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Services;

namespace PortfolioWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IPortfolioService _portfolioService;

        public DashboardController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new AdminDashboardViewModel
            {
                TotalProjects = (await _portfolioService.GetProjectsAsync()).Count(),
                TotalPublications = (await _portfolioService.GetPublicationsAsync()).Count(),
                TotalAchievements = (await _portfolioService.GetAchievementsAsync()).Count(),
                UnreadFeedbacks = (await _portfolioService.GetVisitorFeedbackAsync()).Count(f => !f.IsRead),
                RecentFeedbacks = (await _portfolioService.GetVisitorFeedbackAsync()).Take(5)
            };

            return View(viewModel);
        }
    }

    public class AdminDashboardViewModel
    {
        public int TotalProjects { get; set; }
        public int TotalPublications { get; set; }
        public int TotalAchievements { get; set; }
        public int UnreadFeedbacks { get; set; }
        public IEnumerable<PortfolioWebsite.Models.VisitorFeedback> RecentFeedbacks { get; set; }
    }
}
