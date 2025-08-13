#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Services;
using PortfolioWebsite.Models;

namespace PortfolioWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IGeminiAIService _geminiAIService;

        public DashboardController(IPortfolioService portfolioService, IGeminiAIService geminiAIService)
        {
            _portfolioService = portfolioService;
            _geminiAIService = geminiAIService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new AdminDashboardViewModel
            {
                TotalProjects = (await _portfolioService.GetProjectsAsync()).Count(),
                TotalPublications = (await _portfolioService.GetPublicationsAsync()).Count(),
                TotalAchievements = (await _portfolioService.GetAchievementsAsync()).Count(),
                UnreadFeedback = (await _portfolioService.GetVisitorFeedbackAsync()).Count(f => !f.IsRead),
                RecentFeedback = (await _portfolioService.GetVisitorFeedbackAsync()).Take(5),
                RecentProjects = (await _portfolioService.GetProjectsAsync()).Take(5)
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AIAnalytics()
        {
            var feedbacks = await _portfolioService.GetVisitorFeedbackAsync();
            var viewModel = new AIAnalyticsViewModel
            {
                TotalFeedback = feedbacks.Count(),
                PositiveFeedback = feedbacks.Count(f => f.Sentiment == "Positive"),
                NegativeFeedback = feedbacks.Count(f => f.Sentiment == "Negative"),
                NeutralFeedback = feedbacks.Count(f => f.Sentiment == "Neutral"),
                RecentFeedbackAnalysis = feedbacks.Take(10).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessFeedbackWithAI()
        {
            var unprocessedFeedback = (await _portfolioService.GetVisitorFeedbackAsync())
                .Where(f => string.IsNullOrEmpty(f.AISummary) || string.IsNullOrEmpty(f.Sentiment))
                .ToList();

            foreach (var feedback in unprocessedFeedback)
            {
                if (string.IsNullOrEmpty(feedback.AISummary))
                {
                    feedback.AISummary = await _geminiAIService.SummarizeFeedbackAsync(feedback.Message);
                }

                if (string.IsNullOrEmpty(feedback.Sentiment))
                {
                    feedback.Sentiment = await _geminiAIService.AnalyzeFeedbackSentimentAsync(feedback.Message);
                }

                await _portfolioService.UpdateFeedbackAsync(feedback);
            }

            TempData["SuccessMessage"] = $"Processed {unprocessedFeedback.Count} feedback items with AI analysis.";
            return RedirectToAction(nameof(AIAnalytics));
        }

        public class AdminDashboardViewModel
        {
            public int TotalProjects { get; set; }
            public int TotalPublications { get; set; }
            public int TotalAchievements { get; set; }
            public int UnreadFeedback { get; set; }
            public IEnumerable<VisitorFeedback> RecentFeedback { get; set; } = new List<VisitorFeedback>();
            public IEnumerable<Project> RecentProjects { get; set; } = new List<Project>();
        }

        public class AIAnalyticsViewModel
        {
            public int TotalFeedback { get; set; }
            public int PositiveFeedback { get; set; }
            public int NegativeFeedback { get; set; }
            public int NeutralFeedback { get; set; }
            public List<VisitorFeedback> RecentFeedbackAnalysis { get; set; } = new List<VisitorFeedback>();
        }
    }
}
