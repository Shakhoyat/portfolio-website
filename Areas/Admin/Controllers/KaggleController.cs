using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Data;
using PortfolioWebsite.Models;
using PortfolioWebsite.Services;

namespace PortfolioWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KaggleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IKaggleService _kaggleService;
        private readonly ILogger<KaggleController> _logger;

        public KaggleController(
            ApplicationDbContext context,
            IKaggleService kaggleService,
            ILogger<KaggleController> logger)
        {
            _context = context;
            _kaggleService = kaggleService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var achievements = await _context.KaggleAchievements
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.AchievedDate)
                .ToListAsync();

            var lastSync = await _context.KaggleSyncs
                .OrderByDescending(s => s.LastSyncDate)
                .FirstOrDefaultAsync();

            var viewModel = new KaggleManagementViewModel
            {
                Achievements = achievements,
                LastSync = lastSync,
                TotalAchievements = achievements.Count,
                CompetitionAchievements = achievements.Count(a => a.Type == "Competition"),
                DatasetAchievements = achievements.Count(a => a.Type == "Dataset"),
                NotebookAchievements = achievements.Count(a => a.Type == "Notebook"),
                GoldMedals = achievements.Count(a => a.Medal == "Gold"),
                SilverMedals = achievements.Count(a => a.Medal == "Silver"),
                BronzeMedals = achievements.Count(a => a.Medal == "Bronze")
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SyncKaggleData()
        {
            try
            {
                var username = "shakhoyatshujon"; // Your Kaggle username
                
                // Get Kaggle data
                var profile = await _kaggleService.GetKaggleProfileAsync(username);
                var competitions = await _kaggleService.GetCompetitionsAsync(username);
                var datasets = await _kaggleService.GetDatasetsAsync(username);

                var newAchievements = 0;
                var updatedAchievements = 0;

                // Sync competitions
                foreach (var competition in competitions)
                {
                    var existing = await _context.KaggleAchievements
                        .FirstOrDefaultAsync(a => a.Title == competition.Title && a.Type == "Competition");

                    if (existing == null)
                    {
                        var achievement = new KaggleAchievement
                        {
                            Title = competition.Title,
                            Description = competition.Description,
                            Type = "Competition",
                            Medal = competition.Medal,
                            Rank = competition.Rank,
                            TotalParticipants = competition.TotalTeams,
                            Score = competition.Score,
                            ExternalUrl = competition.CompetitionUrl,
                            AchievedDate = competition.SubmissionDate,
                            Category = competition.Category,
                            IsActive = true,
                            IsFeatured = competition.Medal == "Gold" || competition.Medal == "Silver"
                        };

                        _context.KaggleAchievements.Add(achievement);
                        newAchievements++;
                    }
                    else
                    {
                        existing.Rank = competition.Rank;
                        existing.Score = competition.Score;
                        existing.LastUpdated = DateTime.UtcNow;
                        updatedAchievements++;
                    }
                }

                // Sync datasets
                foreach (var dataset in datasets)
                {
                    var existing = await _context.KaggleAchievements
                        .FirstOrDefaultAsync(a => a.Title == dataset.Title && a.Type == "Dataset");

                    if (existing == null)
                    {
                        var achievement = new KaggleAchievement
                        {
                            Title = dataset.Title,
                            Description = dataset.Description,
                            Type = "Dataset",
                            Medal = dataset.Medal,
                            ExternalUrl = dataset.DatasetUrl,
                            AchievedDate = dataset.CreatedDate,
                            Category = "Published Dataset",
                            IsActive = true,
                            IsFeatured = dataset.Medal == "Gold" || dataset.Medal == "Silver"
                        };

                        _context.KaggleAchievements.Add(achievement);
                        newAchievements++;
                    }
                }

                // Record sync
                var syncRecord = new KaggleSync
                {
                    Username = username,
                    LastSyncDate = DateTime.UtcNow,
                    IsSuccessful = true,
                    NewAchievements = newAchievements,
                    UpdatedAchievements = updatedAchievements
                };

                _context.KaggleSyncs.Add(syncRecord);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Kaggle data synced successfully! {newAchievements} new achievements, {updatedAchievements} updated.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing Kaggle data");
                
                var syncRecord = new KaggleSync
                {
                    Username = "shakhoyatshujon",
                    LastSyncDate = DateTime.UtcNow,
                    IsSuccessful = false,
                    ErrorMessage = ex.Message
                };

                _context.KaggleSyncs.Add(syncRecord);
                await _context.SaveChangesAsync();

                TempData["Error"] = "Failed to sync Kaggle data. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var username = "shakhoyatshujon";
                var profile = await _kaggleService.GetKaggleProfileAsync(username);
                var rankings = await _kaggleService.GetRankingsAsync(username);

                var viewModel = new KaggleProfileViewModel
                {
                    Profile = profile,
                    Rankings = rankings
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Kaggle profile");
                TempData["Error"] = "Failed to load Kaggle profile.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFeatured(int id)
        {
            var achievement = await _context.KaggleAchievements.FindAsync(id);
            if (achievement == null)
            {
                return NotFound();
            }

            achievement.IsFeatured = !achievement.IsFeatured;
            achievement.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Json(new { success = true, featured = achievement.IsFeatured });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var achievement = await _context.KaggleAchievements.FindAsync(id);
            if (achievement == null)
            {
                return NotFound();
            }

            achievement.IsActive = !achievement.IsActive;
            achievement.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Json(new { success = true, active = achievement.IsActive });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var achievement = await _context.KaggleAchievements.FindAsync(id);
            if (achievement == null)
            {
                return NotFound();
            }

            _context.KaggleAchievements.Remove(achievement);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetKaggleStats()
        {
            var achievements = await _context.KaggleAchievements
                .Where(a => a.IsActive)
                .ToListAsync();

            var stats = new
            {
                totalAchievements = achievements.Count,
                competitions = achievements.Count(a => a.Type == "Competition"),
                datasets = achievements.Count(a => a.Type == "Dataset"),
                notebooks = achievements.Count(a => a.Type == "Notebook"),
                goldMedals = achievements.Count(a => a.Medal == "Gold"),
                silverMedals = achievements.Count(a => a.Medal == "Silver"),
                bronzeMedals = achievements.Count(a => a.Medal == "Bronze"),
                featuredItems = achievements.Count(a => a.IsFeatured)
            };

            return Json(stats);
        }
    }

    public class KaggleManagementViewModel
    {
        public List<KaggleAchievement> Achievements { get; set; } = new();
        public KaggleSync? LastSync { get; set; }
        public int TotalAchievements { get; set; }
        public int CompetitionAchievements { get; set; }
        public int DatasetAchievements { get; set; }
        public int NotebookAchievements { get; set; }
        public int GoldMedals { get; set; }
        public int SilverMedals { get; set; }
        public int BronzeMedals { get; set; }
    }

    public class KaggleProfileViewModel
    {
        public KaggleProfile Profile { get; set; } = new();
        public KaggleRankings Rankings { get; set; } = new();
    }
}
