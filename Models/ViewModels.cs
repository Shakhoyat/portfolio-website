#nullable disable
using PortfolioWebsite.Models;

namespace PortfolioWebsite.Models
{
    public class HomeViewModel
    {
        public PersonalInfo PersonalInfo { get; set; }
        public IEnumerable<Project> FeaturedProjects { get; set; }
        public IEnumerable<Skill> Skills { get; set; }
        public IEnumerable<Publication> RecentPublications { get; set; }
        public IEnumerable<SocialLink> SocialLinks { get; set; }
        public IEnumerable<Achievement> RecentAchievements { get; set; }
    }

    public class AboutViewModel
    {
        public PersonalInfo PersonalInfo { get; set; }
        public IEnumerable<Education> Educations { get; set; }
        public IEnumerable<Experience> Experiences { get; set; }
        public IEnumerable<Skill> Skills { get; set; }
    }

    public class AIJobModeViewModel
    {
        public IEnumerable<Skill> Skills { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public JobDescription JobDescription { get; set; } = new JobDescription();
        public IEnumerable<AISkillHighlight> SkillHighlights { get; set; } = new List<AISkillHighlight>();
        public IEnumerable<ProjectRanking> ProjectRankings { get; set; } = new List<ProjectRanking>();
    }

    public class ProjectsViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public string SelectedCategory { get; set; }
        public string SearchTerm { get; set; }
    }

    public class PublicationsViewModel
    {
        public IEnumerable<Publication> Publications { get; set; }
        public string SelectedType { get; set; }
        public int SelectedYear { get; set; }
    }

    public class AchievementsViewModel
    {
        public IEnumerable<Achievement> Achievements { get; set; }
        public string SelectedType { get; set; }
    }

    public class DatasetsViewModel
    {
        public IEnumerable<Dataset> Datasets { get; set; }
        public string SearchTerm { get; set; }
    }

    public class GalleryViewModel
    {
        public IEnumerable<PhotoGallery> Photos { get; set; }
        public string SelectedCategory { get; set; }
    }

    public class ContactViewModel
    {
        public VisitorFeedback Feedback { get; set; } = new VisitorFeedback();
        public PersonalInfo PersonalInfo { get; set; }
        public IEnumerable<SocialLink> SocialLinks { get; set; }
    }
}
