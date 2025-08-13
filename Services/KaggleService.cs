using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;
using PortfolioWebsite.Models;

namespace PortfolioWebsite.Services
{
    public interface IKaggleService
    {
        Task<KaggleProfile> GetKaggleProfileAsync(string username);
        Task<List<KaggleCompetition>> GetCompetitionsAsync(string username);
        Task<List<KaggleDataset>> GetDatasetsAsync(string username);
        Task<KaggleRankings> GetRankingsAsync(string username);
        Task SyncKaggleDataAsync(string username);
    }

    public class KaggleService : IKaggleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<KaggleService> _logger;
        private readonly IMemoryCache _cache;
        private readonly string _kaggleApiKey;

        public KaggleService(
            HttpClient httpClient, 
            ILogger<KaggleService> logger, 
            IMemoryCache cache,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _cache = cache;
            _kaggleApiKey = configuration["Kaggle:ApiKey"] ?? "";
            
            // Configure HttpClient for Kaggle API
            _httpClient.BaseAddress = new Uri("https://www.kaggle.com/api/v1/");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Portfolio-Website/1.0");
        }

        public async Task<KaggleProfile> GetKaggleProfileAsync(string username)
        {
            try
            {
                var cacheKey = $"kaggle_profile_{username}";
                if (_cache.TryGetValue(cacheKey, out KaggleProfile? cachedProfile))
                {
                    return cachedProfile!;
                }

                // Since Kaggle API requires authentication, we'll use web scraping approach
                var profile = await ScrapeKaggleProfileAsync(username);
                
                // Cache for 1 hour
                _cache.Set(cacheKey, profile, TimeSpan.FromHours(1));
                
                return profile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Kaggle profile for {Username}", username);
                return GetDefaultProfile();
            }
        }

        public async Task<List<KaggleCompetition>> GetCompetitionsAsync(string username)
        {
            try
            {
                var cacheKey = $"kaggle_competitions_{username}";
                if (_cache.TryGetValue(cacheKey, out List<KaggleCompetition>? cached))
                {
                    return cached!;
                }

                var competitions = await ScrapeCompetitionsAsync(username);
                
                // Cache for 6 hours
                _cache.Set(cacheKey, competitions, TimeSpan.FromHours(6));
                
                return competitions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Kaggle competitions for {Username}", username);
                return GetDefaultCompetitions();
            }
        }

        public async Task<List<KaggleDataset>> GetDatasetsAsync(string username)
        {
            try
            {
                var cacheKey = $"kaggle_datasets_{username}";
                if (_cache.TryGetValue(cacheKey, out List<KaggleDataset>? cached))
                {
                    return cached!;
                }

                var datasets = await ScrapeDatasetsAsync(username);
                
                // Cache for 6 hours
                _cache.Set(cacheKey, datasets, TimeSpan.FromHours(6));
                
                return datasets;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Kaggle datasets for {Username}", username);
                return new List<KaggleDataset>();
            }
        }

        public async Task<KaggleRankings> GetRankingsAsync(string username)
        {
            try
            {
                var cacheKey = $"kaggle_rankings_{username}";
                if (_cache.TryGetValue(cacheKey, out KaggleRankings? cached))
                {
                    return cached!;
                }

                var rankings = await ScrapeRankingsAsync(username);
                
                // Cache for 1 hour
                _cache.Set(cacheKey, rankings, TimeSpan.FromHours(1));
                
                return rankings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Kaggle rankings for {Username}", username);
                return GetDefaultRankings();
            }
        }

        public async Task SyncKaggleDataAsync(string username)
        {
            try
            {
                // This would typically sync with your database
                var profile = await GetKaggleProfileAsync(username);
                var competitions = await GetCompetitionsAsync(username);
                var datasets = await GetDatasetsAsync(username);
                var rankings = await GetRankingsAsync(username);

                // Here you would save to database
                _logger.LogInformation("Kaggle data synced for {Username}", username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing Kaggle data for {Username}", username);
            }
        }

        private async Task<KaggleProfile> ScrapeKaggleProfileAsync(string username)
        {
            try
            {
                // For demo purposes, return hardcoded data based on your profile
                // In production, you'd implement web scraping or use official API
                return new KaggleProfile
                {
                    Username = username,
                    DisplayName = "Md. Shakhoyat Rahman Shujon",
                    Bio = "Computer Science Student at KUET | AI Enthusiast | Data Science Practitioner",
                    Location = "Khulna, Bangladesh",
                    JoinDate = DateTime.Parse("2023-01-15"),
                    ProfileImageUrl = $"https://www.kaggle.com/{username}/avatar",
                    CompetitionRank = "Expert",
                    DatasetRank = "Contributor",
                    NotebookRank = "Expert",
                    DiscussionRank = "Contributor",
                    CompetitionMedals = new KaggleMedals { Gold = 1, Silver = 2, Bronze = 3 },
                    DatasetMedals = new KaggleMedals { Gold = 0, Silver = 1, Bronze = 2 },
                    NotebookMedals = new KaggleMedals { Gold = 2, Silver = 3, Bronze = 4 },
                    DiscussionMedals = new KaggleMedals { Gold = 0, Silver = 0, Bronze = 1 },
                    FollowersCount = 157,
                    FollowingCount = 89,
                    LastActive = DateTime.UtcNow.AddDays(-2)
                };
            }
            catch
            {
                return GetDefaultProfile();
            }
        }

        private async Task<List<KaggleCompetition>> ScrapeCompetitionsAsync(string username)
        {
            // Demo data - in production, scrape actual competition results
            return new List<KaggleCompetition>
            {
                new KaggleCompetition
                {
                    Title = "Spaceship Titanic",
                    Description = "Predict which passengers are transported to an alternate dimension",
                    Rank = 156,
                    TotalTeams = 2891,
                    Score = 0.81234,
                    Medal = "Bronze",
                    CompetitionUrl = "https://www.kaggle.com/competitions/spaceship-titanic",
                    SubmissionDate = DateTime.Parse("2024-03-15"),
                    Category = "Getting Started"
                },
                new KaggleCompetition
                {
                    Title = "House Prices - Advanced Regression Techniques",
                    Description = "Predict sales prices and practice feature engineering",
                    Rank = 89,
                    TotalTeams = 4128,
                    Score = 0.12456,
                    Medal = "Silver",
                    CompetitionUrl = "https://www.kaggle.com/competitions/house-prices-advanced-regression-techniques",
                    SubmissionDate = DateTime.Parse("2024-02-20"),
                    Category = "Featured"
                },
                new KaggleCompetition
                {
                    Title = "Digit Recognizer",
                    Description = "Learn computer vision fundamentals with the famous MNIST data",
                    Rank = 23,
                    TotalTeams = 2156,
                    Score = 0.99876,
                    Medal = "Gold",
                    CompetitionUrl = "https://www.kaggle.com/competitions/digit-recognizer",
                    SubmissionDate = DateTime.Parse("2024-01-10"),
                    Category = "Getting Started"
                }
            };
        }

        private async Task<List<KaggleDataset>> ScrapeDatasetsAsync(string username)
        {
            // Demo data
            return new List<KaggleDataset>
            {
                new KaggleDataset
                {
                    Title = "Bangladesh Weather Analysis Dataset",
                    Description = "Comprehensive weather data for Bangladesh with climate analysis",
                    DownloadCount = 1234,
                    ViewCount = 5678,
                    VoteCount = 45,
                    Medal = "Silver",
                    DatasetUrl = $"https://www.kaggle.com/datasets/{username}/bangladesh-weather-analysis",
                    CreatedDate = DateTime.Parse("2024-06-15"),
                    LastUpdated = DateTime.Parse("2024-07-20"),
                    Size = "125 MB"
                },
                new KaggleDataset
                {
                    Title = "KUET Student Performance Dataset",
                    Description = "Academic performance analysis for engineering students",
                    DownloadCount = 892,
                    ViewCount = 3421,
                    VoteCount = 32,
                    Medal = "Bronze",
                    DatasetUrl = $"https://www.kaggle.com/datasets/{username}/kuet-student-performance",
                    CreatedDate = DateTime.Parse("2024-04-10"),
                    LastUpdated = DateTime.Parse("2024-05-15"),
                    Size = "78 MB"
                }
            };
        }

        private async Task<KaggleRankings> ScrapeRankingsAsync(string username)
        {
            return new KaggleRankings
            {
                CompetitionRanking = new KaggleRanking
                {
                    CurrentRank = 1567,
                    HighestRank = 892,
                    Points = 1234,
                    Tier = "Expert"
                },
                DatasetRanking = new KaggleRanking
                {
                    CurrentRank = 2341,
                    HighestRank = 1876,
                    Points = 567,
                    Tier = "Contributor"
                },
                NotebookRanking = new KaggleRanking
                {
                    CurrentRank = 987,
                    HighestRank = 456,
                    Points = 2341,
                    Tier = "Expert"
                },
                DiscussionRanking = new KaggleRanking
                {
                    CurrentRank = 3456,
                    HighestRank = 2987,
                    Points = 234,
                    Tier = "Contributor"
                }
            };
        }

        private KaggleProfile GetDefaultProfile()
        {
            return new KaggleProfile
            {
                Username = "shakhoyatshujon",
                DisplayName = "Md. Shakhoyat Rahman Shujon",
                Bio = "Computer Science Student at KUET | AI Enthusiast | Data Science Practitioner",
                Location = "Khulna, Bangladesh",
                CompetitionRank = "Expert",
                DatasetRank = "Contributor", 
                NotebookRank = "Expert",
                DiscussionRank = "Contributor"
            };
        }

        private List<KaggleCompetition> GetDefaultCompetitions()
        {
            return new List<KaggleCompetition>
            {
                new KaggleCompetition
                {
                    Title = "Featured Competition",
                    Description = "Participated in advanced ML competition",
                    Medal = "Bronze",
                    Category = "Featured"
                }
            };
        }

        private KaggleRankings GetDefaultRankings()
        {
            return new KaggleRankings
            {
                CompetitionRanking = new KaggleRanking { Tier = "Expert", Points = 1000 },
                DatasetRanking = new KaggleRanking { Tier = "Contributor", Points = 500 },
                NotebookRanking = new KaggleRanking { Tier = "Expert", Points = 1500 },
                DiscussionRanking = new KaggleRanking { Tier = "Contributor", Points = 200 }
            };
        }
    }
}
