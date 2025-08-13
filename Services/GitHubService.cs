#nullable disable
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PortfolioWebsite.Data;
using PortfolioWebsite.Models;

namespace PortfolioWebsite.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public GitHubService(HttpClient httpClient, ApplicationDbContext context, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _context = context;
            _configuration = configuration;
            
            // Configure HttpClient for GitHub API
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Portfolio-Website");
            var token = _configuration["GitHub:Token"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"token {token}");
            }
        }

        public async Task<IEnumerable<GitHubRepository>> GetRepositoriesAsync(string username)
        {
            try
            {
                var url = $"https://api.github.com/users/{username}/repos?sort=updated&per_page=100";
                var response = await _httpClient.GetStringAsync(url);
                var repositories = JsonConvert.DeserializeObject<List<GitHubApiRepository>>(response);

                return repositories.Select(repo => new GitHubRepository
                {
                    Name = repo.Name,
                    Description = repo.Description,
                    HtmlUrl = repo.HtmlUrl,
                    Language = repo.Language,
                    StargazersCount = repo.StargazersCount,
                    ForksCount = repo.ForksCount,
                    CreatedAt = repo.CreatedAt,
                    UpdatedAt = repo.UpdatedAt,
                    Topics = repo.Topics ?? new string[0]
                });
            }
            catch (Exception)
            {
                return new List<GitHubRepository>();
            }
        }

        public async Task<GitHubRepository> GetRepositoryDetailsAsync(string username, string repoName)
        {
            try
            {
                var url = $"https://api.github.com/repos/{username}/{repoName}";
                var response = await _httpClient.GetStringAsync(url);
                var repo = JsonConvert.DeserializeObject<GitHubApiRepository>(response);

                return new GitHubRepository
                {
                    Name = repo.Name,
                    Description = repo.Description,
                    HtmlUrl = repo.HtmlUrl,
                    Language = repo.Language,
                    StargazersCount = repo.StargazersCount,
                    ForksCount = repo.ForksCount,
                    CreatedAt = repo.CreatedAt,
                    UpdatedAt = repo.UpdatedAt,
                    Topics = repo.Topics ?? new string[0]
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task SyncProjectsWithGitHubAsync(string username)
        {
            var repositories = await GetRepositoriesAsync(username);
            
            foreach (var repo in repositories)
            {
                // Check if project already exists
                var existingProject = await _context.Projects
                    .FirstOrDefaultAsync(p => p.GitHubUrl == repo.HtmlUrl);

                if (existingProject == null)
                {
                    // Create new project from repository
                    var project = new Project
                    {
                        Title = repo.Name,
                        ShortDescription = repo.Description ?? "GitHub Repository",
                        DetailedDescription = $"Repository: {repo.Description}\n\nLanguage: {repo.Language}\nStars: {repo.StargazersCount}\nForks: {repo.ForksCount}",
                        GitHubUrl = repo.HtmlUrl,
                        TechnologiesUsed = repo.Language,
                        Status = ProjectStatus.Completed,
                        Category = DetermineProjectCategory(repo.Language, repo.Topics),
                        StartDate = repo.CreatedAt,
                        EndDate = repo.UpdatedAt,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsFeatured = repo.StargazersCount > 5 // Feature popular repositories
                    };

                    _context.Projects.Add(project);
                }
                else
                {
                    // Update existing project
                    existingProject.ShortDescription = repo.Description ?? existingProject.ShortDescription;
                    existingProject.TechnologiesUsed = repo.Language ?? existingProject.TechnologiesUsed;
                    existingProject.EndDate = repo.UpdatedAt;
                    existingProject.IsFeatured = repo.StargazersCount > 5;
                }
            }

            await _context.SaveChangesAsync();
        }

        private ProjectCategory DetermineProjectCategory(string language, string[] topics)
        {
            if (topics != null)
            {
                if (topics.Any(t => t.Contains("machine-learning") || t.Contains("ai") || t.Contains("ml")))
                    return ProjectCategory.MachineLearning;
                if (topics.Any(t => t.Contains("data-science") || t.Contains("analytics")))
                    return ProjectCategory.DataScience;
                if (topics.Any(t => t.Contains("web") || t.Contains("frontend") || t.Contains("backend")))
                    return ProjectCategory.WebDevelopment;
                if (topics.Any(t => t.Contains("mobile") || t.Contains("android") || t.Contains("ios")))
                    return ProjectCategory.MobileApp;
                if (topics.Any(t => t.Contains("nlp") || t.Contains("natural-language")))
                    return ProjectCategory.NLP;
                if (topics.Any(t => t.Contains("computer-vision") || t.Contains("opencv")))
                    return ProjectCategory.ComputerVision;
            }

            // Fallback to language-based categorization
            return language?.ToLower() switch
            {
                "python" => ProjectCategory.DataScience,
                "javascript" => ProjectCategory.WebDevelopment,
                "typescript" => ProjectCategory.WebDevelopment,
                "html" => ProjectCategory.WebDevelopment,
                "css" => ProjectCategory.WebDevelopment,
                "java" => ProjectCategory.WebDevelopment,
                "swift" => ProjectCategory.MobileApp,
                "kotlin" => ProjectCategory.MobileApp,
                "dart" => ProjectCategory.MobileApp,
                _ => ProjectCategory.Other
            };
        }

        // GitHub API response models
        private class GitHubApiRepository
        {
            public string Name { get; set; }
            public string Description { get; set; }
            [JsonProperty("html_url")]
            public string HtmlUrl { get; set; }
            public string Language { get; set; }
            [JsonProperty("stargazers_count")]
            public int StargazersCount { get; set; }
            [JsonProperty("forks_count")]
            public int ForksCount { get; set; }
            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set; }
            [JsonProperty("updated_at")]
            public DateTime UpdatedAt { get; set; }
            public string[] Topics { get; set; }
        }
    }

    public class ScholarService : IScholarService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ScholarService(HttpClient httpClient, ApplicationDbContext context, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<ScholarPublication>> GetPublicationsAsync(string authorId)
        {
            // Note: Google Scholar doesn't have an official API
            // This is a placeholder for potential integration with Scholar scraping services
            // or alternative academic APIs like Semantic Scholar, ORCID, etc.
            
            try
            {
                // Placeholder implementation - in production, integrate with:
                // - Semantic Scholar API
                // - ORCID API
                // - CrossRef API
                // - Or a Scholar scraping service
                
                return new List<ScholarPublication>();
            }
            catch (Exception)
            {
                return new List<ScholarPublication>();
            }
        }

        public async Task<ScholarProfile> GetProfileAsync(string authorId)
        {
            try
            {
                // Placeholder implementation
                return new ScholarProfile
                {
                    Name = "Author Name",
                    Affiliation = "University/Organization",
                    TotalCitations = 0,
                    HIndex = 0,
                    Interests = new string[0]
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task SyncPublicationsAsync(string authorId)
        {
            var publications = await GetPublicationsAsync(authorId);
            
            foreach (var pub in publications)
            {
                // Check if publication already exists
                var existingPub = await _context.Publications
                    .FirstOrDefaultAsync(p => p.Title == pub.Title);

                if (existingPub == null)
                {
                    // Create new publication
                    var publication = new Publication
                    {
                        Title = pub.Title,
                        Authors = pub.Authors,
                        Journal = pub.Venue,
                        PublicationDate = new DateTime(pub.Year, 1, 1),
                        GoogleScholarUrl = pub.Url,
                        CitationCount = pub.Citations,
                        Type = PublicationType.JournalArticle,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true
                    };

                    _context.Publications.Add(publication);
                }
                else
                {
                    // Update citation count
                    existingPub.CitationCount = pub.Citations;
                    existingPub.GoogleScholarUrl = pub.Url;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
