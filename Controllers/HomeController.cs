#nullable disable
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Models;
using PortfolioWebsite.Services;

namespace PortfolioWebsite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPortfolioService _portfolioService;

    public HomeController(ILogger<HomeController> logger, IPortfolioService portfolioService)
    {
        _logger = logger;
        _portfolioService = portfolioService;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            PersonalInfo = await _portfolioService.GetPersonalInfoAsync(),
            FeaturedProjects = await _portfolioService.GetFeaturedProjectsAsync(),
            Skills = await _portfolioService.GetSkillsAsync(),
            RecentPublications = (await _portfolioService.GetPublicationsAsync()).Take(3),
            SocialLinks = await _portfolioService.GetSocialLinksAsync()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> About()
    {
        var viewModel = new AboutViewModel
        {
            PersonalInfo = await _portfolioService.GetPersonalInfoAsync(),
            Educations = await _portfolioService.GetEducationsAsync(),
            Experiences = await _portfolioService.GetExperiencesAsync(),
            Skills = await _portfolioService.GetSkillsAsync()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Projects()
    {
        var projects = await _portfolioService.GetProjectsAsync();
        return View(projects);
    }

    public async Task<IActionResult> ProjectDetails(int id)
    {
        var project = await _portfolioService.GetProjectByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }

    public async Task<IActionResult> Publications()
    {
        var publications = await _portfolioService.GetPublicationsAsync();
        return View(publications);
    }

    public async Task<IActionResult> Achievements()
    {
        var achievements = await _portfolioService.GetAchievementsAsync();
        return View(achievements);
    }

    public async Task<IActionResult> Datasets()
    {
        var datasets = await _portfolioService.GetDatasetsAsync();
        return View(datasets);
    }

    public async Task<IActionResult> Gallery()
    {
        var photos = await _portfolioService.GetPhotoGalleryAsync();
        return View(photos);
    }

    public IActionResult Contact()
    {
        return View(new VisitorFeedback());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(VisitorFeedback feedback)
    {
        if (ModelState.IsValid)
        {
            feedback.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            feedback.UserAgent = HttpContext.Request.Headers["User-Agent"];
            
            await _portfolioService.CreateFeedbackAsync(feedback);
            
            TempData["SuccessMessage"] = "Thank you for your message! I'll get back to you soon.";
            return RedirectToAction(nameof(Contact));
        }

        return View(feedback);
    }

    [HttpGet]
    public async Task<IActionResult> AIJobMode()
    {
        var viewModel = new AIJobModeViewModel
        {
            Skills = await _portfolioService.GetSkillsAsync(),
            Projects = await _portfolioService.GetProjectsAsync()
        };
        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
