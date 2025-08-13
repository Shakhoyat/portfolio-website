#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Services;
using PortfolioWebsite.Models;

namespace PortfolioWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProjectsController : Controller
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IGitHubService _gitHubService;

        public ProjectsController(IPortfolioService portfolioService, IGitHubService gitHubService)
        {
            _portfolioService = portfolioService;
            _gitHubService = gitHubService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _portfolioService.GetProjectsAsync();
            return View(projects);
        }

        public IActionResult Create()
        {
            return View(new Project());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                await _portfolioService.CreateProjectAsync(project);
                TempData["SuccessMessage"] = "Project created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var project = await _portfolioService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _portfolioService.UpdateProjectAsync(project);
                TempData["SuccessMessage"] = "Project updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public async Task<IActionResult> Details(int id)
        {
            var project = await _portfolioService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _portfolioService.DeleteProjectAsync(id);
            TempData["SuccessMessage"] = "Project deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SyncWithGitHub()
        {
            try
            {
                var githubUsername = "YOUR_GITHUB_USERNAME"; // Get from configuration
                await _gitHubService.SyncProjectsWithGitHubAsync(githubUsername);
                TempData["SuccessMessage"] = "Projects synced with GitHub successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error syncing with GitHub: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFeatured(int id)
        {
            var project = await _portfolioService.GetProjectByIdAsync(id);
            if (project != null)
            {
                project.IsFeatured = !project.IsFeatured;
                await _portfolioService.UpdateProjectAsync(project);
                
                var status = project.IsFeatured ? "featured" : "unfeatured";
                TempData["SuccessMessage"] = $"Project {status} successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
