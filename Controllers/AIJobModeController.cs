#nullable disable
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Services;
using PortfolioWebsite.Models;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Data;

namespace PortfolioWebsite.Controllers
{
    public class AIJobModeController : Controller
    {
        private readonly IGeminiAIService _geminiAIService;
        private readonly IPortfolioService _portfolioService;
        private readonly ApplicationDbContext _context;

        public AIJobModeController(IGeminiAIService geminiAIService, IPortfolioService portfolioService, ApplicationDbContext context)
        {
            _geminiAIService = geminiAIService;
            _portfolioService = portfolioService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new AIJobModeViewModel
            {
                Skills = await _portfolioService.GetSkillsAsync(),
                Projects = await _portfolioService.GetProjectsAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AnalyzeJob(AIJobModeViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.JobDescription.Description))
            {
                ModelState.AddModelError("JobDescription.Description", "Please enter a job description to analyze.");
                model.Skills = await _portfolioService.GetSkillsAsync();
                model.Projects = await _portfolioService.GetProjectsAsync();
                return View("Index", model);
            }

            // Save job description
            var jobDescription = new JobDescription
            {
                JobTitle = model.JobDescription.JobTitle ?? "Untitled Position",
                Company = model.JobDescription.Company ?? "Unknown Company",
                Description = model.JobDescription.Description,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            // AI Analysis
            jobDescription.AIAnalysis = await _geminiAIService.AnalyzeJobDescriptionAsync(jobDescription.Description);

            _context.JobDescriptions.Add(jobDescription);
            await _context.SaveChangesAsync();

            // Generate skill highlights
            var skillHighlights = await _geminiAIService.GenerateSkillHighlightsAsync(jobDescription.Id);
            foreach (var highlight in skillHighlights)
            {
                _context.AISkillHighlights.Add(highlight);
            }

            // Generate project rankings
            var projectRankings = await _geminiAIService.RankProjectsForJobAsync(jobDescription.Id);
            foreach (var ranking in projectRankings)
            {
                _context.ProjectRankings.Add(ranking);
            }

            await _context.SaveChangesAsync();

            // Prepare result view model
            var resultViewModel = new AIJobAnalysisResultViewModel
            {
                JobDescription = jobDescription,
                SkillHighlights = await _context.AISkillHighlights
                    .Include(sh => sh.Skill)
                    .Where(sh => sh.JobDescriptionId == jobDescription.Id)
                    .OrderByDescending(sh => sh.RelevanceScore)
                    .ToListAsync(),
                ProjectRankings = await _context.ProjectRankings
                    .Include(pr => pr.Project)
                    .Where(pr => pr.JobDescriptionId == jobDescription.Id)
                    .OrderByDescending(pr => pr.RelevanceScore)
                    .ToListAsync(),
                RecommendedProjects = await _context.ProjectRankings
                    .Include(pr => pr.Project)
                    .Where(pr => pr.JobDescriptionId == jobDescription.Id && pr.RelevanceScore > 60)
                    .OrderByDescending(pr => pr.RelevanceScore)
                    .Select(pr => pr.Project)
                    .ToListAsync(),
                TopSkills = await _context.AISkillHighlights
                    .Include(sh => sh.Skill)
                    .Where(sh => sh.JobDescriptionId == jobDescription.Id && sh.RelevanceScore > 50)
                    .OrderByDescending(sh => sh.RelevanceScore)
                    .Select(sh => sh.Skill)
                    .ToListAsync()
            };

            return View("AnalysisResult", resultViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> JobHistory()
        {
            var jobDescriptions = await _context.JobDescriptions
                .Where(jd => jd.IsActive)
                .OrderByDescending(jd => jd.CreatedDate)
                .ToListAsync();

            return View(jobDescriptions);
        }

        [HttpGet]
        public async Task<IActionResult> ViewAnalysis(int id)
        {
            var jobDescription = await _context.JobDescriptions.FindAsync(id);
            if (jobDescription == null)
            {
                return NotFound();
            }

            var resultViewModel = new AIJobAnalysisResultViewModel
            {
                JobDescription = jobDescription,
                SkillHighlights = await _context.AISkillHighlights
                    .Include(sh => sh.Skill)
                    .Where(sh => sh.JobDescriptionId == id)
                    .OrderByDescending(sh => sh.RelevanceScore)
                    .ToListAsync(),
                ProjectRankings = await _context.ProjectRankings
                    .Include(pr => pr.Project)
                    .Where(pr => pr.JobDescriptionId == id)
                    .OrderByDescending(pr => pr.RelevanceScore)
                    .ToListAsync(),
                RecommendedProjects = await _context.ProjectRankings
                    .Include(pr => pr.Project)
                    .Where(pr => pr.JobDescriptionId == id && pr.RelevanceScore > 60)
                    .OrderByDescending(pr => pr.RelevanceScore)
                    .Select(pr => pr.Project)
                    .ToListAsync(),
                TopSkills = await _context.AISkillHighlights
                    .Include(sh => sh.Skill)
                    .Where(sh => sh.JobDescriptionId == id && sh.RelevanceScore > 50)
                    .OrderByDescending(sh => sh.RelevanceScore)
                    .Select(sh => sh.Skill)
                    .ToListAsync()
            };

            return View("AnalysisResult", resultViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteJobAnalysis(int id)
        {
            var jobDescription = await _context.JobDescriptions.FindAsync(id);
            if (jobDescription != null)
            {
                jobDescription.IsActive = false;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Job analysis deleted successfully.";
            }

            return RedirectToAction(nameof(JobHistory));
        }

        [HttpGet]
        public async Task<IActionResult> OptimizeForJob(int jobId)
        {
            var jobDescription = await _context.JobDescriptions.FindAsync(jobId);
            if (jobDescription == null)
            {
                return NotFound();
            }

            var optimizationViewModel = new JobOptimizationViewModel
            {
                JobDescription = jobDescription,
                RecommendedSkillUpdates = await GetRecommendedSkillUpdates(jobId),
                SuggestedProjectHighlights = await GetSuggestedProjectHighlights(jobId),
                RecommendedProfileUpdates = await GetRecommendedProfileUpdates(jobId)
            };

            return View(optimizationViewModel);
        }

        private async Task<List<string>> GetRecommendedSkillUpdates(int jobId)
        {
            var topSkills = await _context.AISkillHighlights
                .Include(sh => sh.Skill)
                .Where(sh => sh.JobDescriptionId == jobId && sh.RelevanceScore > 70)
                .OrderByDescending(sh => sh.RelevanceScore)
                .Select(sh => $"Emphasize {sh.Skill.Name} - {sh.HighlightReason}")
                .ToListAsync();

            return topSkills;
        }

        private async Task<List<string>> GetSuggestedProjectHighlights(int jobId)
        {
            var topProjects = await _context.ProjectRankings
                .Include(pr => pr.Project)
                .Where(pr => pr.JobDescriptionId == jobId && pr.RelevanceScore > 70)
                .OrderByDescending(pr => pr.RelevanceScore)
                .Select(pr => $"Feature {pr.Project.Title} - {pr.RankingReason}")
                .ToListAsync();

            return topProjects;
        }

        private async Task<List<string>> GetRecommendedProfileUpdates(int jobId)
        {
            var jobDescription = await _context.JobDescriptions.FindAsync(jobId);
            var recommendations = new List<string>();

            if (jobDescription != null)
            {
                recommendations.Add($"Update bio to emphasize experience relevant to {jobDescription.JobTitle}");
                recommendations.Add("Highlight recent projects that align with job requirements");
                recommendations.Add("Update skills section to prioritize most relevant technologies");
                recommendations.Add("Consider adding case studies for top-ranked projects");
            }

            return recommendations;
        }
    }

    public class AIJobAnalysisResultViewModel
    {
        public JobDescription JobDescription { get; set; }
        public List<AISkillHighlight> SkillHighlights { get; set; } = new List<AISkillHighlight>();
        public List<ProjectRanking> ProjectRankings { get; set; } = new List<ProjectRanking>();
        public List<Project> RecommendedProjects { get; set; } = new List<Project>();
        public List<Skill> TopSkills { get; set; } = new List<Skill>();
    }

    public class JobOptimizationViewModel
    {
        public JobDescription JobDescription { get; set; }
        public List<string> RecommendedSkillUpdates { get; set; } = new List<string>();
        public List<string> SuggestedProjectHighlights { get; set; } = new List<string>();
        public List<string> RecommendedProfileUpdates { get; set; } = new List<string>();
    }
}
