#nullable disable
using PortfolioWebsite.Models;

namespace PortfolioWebsite.Services
{
    public interface IPortfolioService
    {
        // Personal Info
        Task<PersonalInfo> GetPersonalInfoAsync();
        Task UpdatePersonalInfoAsync(PersonalInfo personalInfo);

        // Education
        Task<IEnumerable<Education>> GetEducationsAsync();
        Task<Education> GetEducationByIdAsync(int id);
        Task CreateEducationAsync(Education education);
        Task UpdateEducationAsync(Education education);
        Task DeleteEducationAsync(int id);

        // Experience
        Task<IEnumerable<Experience>> GetExperiencesAsync();
        Task<Experience> GetExperienceByIdAsync(int id);
        Task CreateExperienceAsync(Experience experience);
        Task UpdateExperienceAsync(Experience experience);
        Task DeleteExperienceAsync(int id);

        // Skills
        Task<IEnumerable<Skill>> GetSkillsAsync();
        Task<IEnumerable<Skill>> GetSkillsByCategoryAsync(string category);
        Task<Skill> GetSkillByIdAsync(int id);
        Task CreateSkillAsync(Skill skill);
        Task UpdateSkillAsync(Skill skill);
        Task DeleteSkillAsync(int id);

        // Projects
        Task<IEnumerable<Project>> GetProjectsAsync();
        Task<IEnumerable<Project>> GetFeaturedProjectsAsync();
        Task<Project> GetProjectByIdAsync(int id);
        Task CreateProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(int id);

        // Publications
        Task<IEnumerable<Publication>> GetPublicationsAsync();
        Task<Publication> GetPublicationByIdAsync(int id);
        Task CreatePublicationAsync(Publication publication);
        Task UpdatePublicationAsync(Publication publication);
        Task DeletePublicationAsync(int id);

        // Achievements
        Task<IEnumerable<Achievement>> GetAchievementsAsync();
        Task<Achievement> GetAchievementByIdAsync(int id);
        Task CreateAchievementAsync(Achievement achievement);
        Task UpdateAchievementAsync(Achievement achievement);
        Task DeleteAchievementAsync(int id);

        // Datasets
        Task<IEnumerable<Dataset>> GetDatasetsAsync();
        Task<Dataset> GetDatasetByIdAsync(int id);
        Task CreateDatasetAsync(Dataset dataset);
        Task UpdateDatasetAsync(Dataset dataset);
        Task DeleteDatasetAsync(int id);

        // Social Links
        Task<IEnumerable<SocialLink>> GetSocialLinksAsync();
        Task<SocialLink> GetSocialLinkByIdAsync(int id);
        Task CreateSocialLinkAsync(SocialLink socialLink);
        Task UpdateSocialLinkAsync(SocialLink socialLink);
        Task DeleteSocialLinkAsync(int id);

        // Photo Gallery
        Task<IEnumerable<PhotoGallery>> GetPhotoGalleryAsync();
        Task<PhotoGallery> GetPhotoByIdAsync(int id);
        Task CreatePhotoAsync(PhotoGallery photo);
        Task UpdatePhotoAsync(PhotoGallery photo);
        Task DeletePhotoAsync(int id);

        // Visitor Feedback
        Task<IEnumerable<VisitorFeedback>> GetVisitorFeedbackAsync();
        Task<VisitorFeedback> GetFeedbackByIdAsync(int id);
        Task CreateFeedbackAsync(VisitorFeedback feedback);
        Task UpdateFeedbackAsync(VisitorFeedback feedback);
        Task DeleteFeedbackAsync(int id);
        Task MarkFeedbackAsReadAsync(int id);
    }

    public interface IGeminiAIService
    {
        Task<string> AnalyzeJobDescriptionAsync(string jobDescription);
        Task<IEnumerable<AISkillHighlight>> GenerateSkillHighlightsAsync(int jobDescriptionId);
        Task<IEnumerable<ProjectRanking>> RankProjectsForJobAsync(int jobDescriptionId);
        Task<string> SummarizeFeedbackAsync(string feedback);
        Task<string> AnalyzeFeedbackSentimentAsync(string feedback);
    }

    public interface IGitHubService
    {
        Task<IEnumerable<GitHubRepository>> GetRepositoriesAsync(string username);
        Task<GitHubRepository> GetRepositoryDetailsAsync(string username, string repoName);
        Task SyncProjectsWithGitHubAsync(string username);
    }

    public interface IScholarService
    {
        Task<IEnumerable<ScholarPublication>> GetPublicationsAsync(string authorId);
        Task<ScholarProfile> GetProfileAsync(string authorId);
        Task SyncPublicationsAsync(string authorId);
    }

    // DTOs for external services
    public class GitHubRepository
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HtmlUrl { get; set; }
        public string Language { get; set; }
        public int StargazersCount { get; set; }
        public int ForksCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string[] Topics { get; set; }
    }

    public class ScholarPublication
    {
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Venue { get; set; }
        public int Year { get; set; }
        public int Citations { get; set; }
        public string Url { get; set; }
    }

    public class ScholarProfile
    {
        public string Name { get; set; }
        public string Affiliation { get; set; }
        public int TotalCitations { get; set; }
        public int HIndex { get; set; }
        public string[] Interests { get; set; }
    }
}
