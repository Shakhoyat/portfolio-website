#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioWebsite.Models
{
    public class PersonalInfo
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        
        [StringLength(100)]
        public string Title { get; set; }
        
        [StringLength(500)]
        public string Bio { get; set; }
        
        [StringLength(2000)]
        public string AboutMe { get; set; }
        
        [StringLength(200)]
        public string Email { get; set; }
        
        [StringLength(20)]
        public string Phone { get; set; }
        
        [StringLength(200)]
        public string Location { get; set; }
        
        [StringLength(500)]
        public string ProfileImageUrl { get; set; }
        
        [StringLength(500)]
        public string ResumeUrl { get; set; }
        
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }

    public class Education
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Institution { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Degree { get; set; }
        
        [StringLength(100)]
        public string FieldOfStudy { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        [Column(TypeName = "decimal(3,2)")]
        public decimal? GPA { get; set; }
        
        [StringLength(1000)]
        public string Description { get; set; }
        
        [StringLength(500)]
        public string LogoUrl { get; set; }
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

    public class Experience
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Company { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Position { get; set; }
        
        [StringLength(100)]
        public string Location { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        [StringLength(2000)]
        public string Description { get; set; }
        
        [StringLength(500)]
        public string CompanyLogoUrl { get; set; }
        
        [StringLength(500)]
        public string CompanyUrl { get; set; }
        
        public ExperienceType Type { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

    public enum ExperienceType
    {
        FullTime,
        PartTime,
        Internship,
        Freelance,
        Volunteer,
        Research
    }

    public class Skill
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(50)]
        public string Category { get; set; }
        
        [Range(1, 10)]
        public int ProficiencyLevel { get; set; }
        
        [StringLength(500)]
        public string Description { get; set; }
        
        [StringLength(500)]
        public string IconUrl { get; set; }
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

    public class Project
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        
        [StringLength(500)]
        public string ShortDescription { get; set; }
        
        [StringLength(5000)]
        public string DetailedDescription { get; set; }
        
        [StringLength(500)]
        public string ImageUrl { get; set; }
        
        [StringLength(500)]
        public string GitHubUrl { get; set; }
        
        [StringLength(500)]
        public string LiveDemoUrl { get; set; }
        
        [StringLength(500)]
        public string TechnologiesUsed { get; set; }
        
        public ProjectStatus Status { get; set; }
        public ProjectCategory Category { get; set; }
        
        [Range(1, 10)]
        public int DifficultyLevel { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; } = true;
        
        // AI Ranking Integration
        public virtual ICollection<ProjectRanking> ProjectRankings { get; set; } = new List<ProjectRanking>();
    }

    public enum ProjectStatus
    {
        Planning,
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }

    public enum ProjectCategory
    {
        MachineLearning,
        DataScience,
        WebDevelopment,
        MobileApp,
        Research,
        NLP,
        ComputerVision,
        DataAnalysis,
        Other
    }

    public class Publication
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Title { get; set; }
        
        [StringLength(200)]
        public string Authors { get; set; }
        
        [StringLength(200)]
        public string Journal { get; set; }
        
        [StringLength(200)]
        public string Conference { get; set; }
        
        public DateTime PublicationDate { get; set; }
        
        [StringLength(2000)]
        public string Abstract { get; set; }
        
        [StringLength(500)]
        public string DOI { get; set; }
        
        [StringLength(500)]
        public string PdfUrl { get; set; }
        
        [StringLength(500)]
        public string GoogleScholarUrl { get; set; }
        
        public int CitationCount { get; set; }
        
        public PublicationType Type { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

    public enum PublicationType
    {
        JournalArticle,
        ConferencePaper,
        Workshop,
        BookChapter,
        TechReport,
        Thesis,
        Preprint
    }

    public class Achievement
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        
        [StringLength(200)]
        public string Organization { get; set; }
        
        public DateTime DateReceived { get; set; }
        
        [StringLength(1000)]
        public string Description { get; set; }
        
        [StringLength(500)]
        public string ImageUrl { get; set; }
        
        [StringLength(500)]
        public string CertificateUrl { get; set; }
        
        public AchievementType Type { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

    public enum AchievementType
    {
        Award,
        Scholarship,
        Certification,
        Competition,
        Grant,
        Recognition,
        Other
    }

    public class Dataset
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        
        [StringLength(1000)]
        public string Description { get; set; }
        
        [StringLength(500)]
        public string KaggleUrl { get; set; }
        
        [StringLength(500)]
        public string GitHubUrl { get; set; }
        
        [StringLength(500)]
        public string DownloadUrl { get; set; }
        
        [StringLength(100)]
        public string Format { get; set; }
        
        [StringLength(50)]
        public string Size { get; set; }
        
        public int Downloads { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
    }

    public class SocialLink
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Platform { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Url { get; set; }
        
        [StringLength(100)]
        public string Username { get; set; }
        
        [StringLength(500)]
        public string IconUrl { get; set; }
        
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

    public class PhotoGallery
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        
        [StringLength(1000)]
        public string Description { get; set; }
        
        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; }
        
        [StringLength(500)]
        public string ThumbnailUrl { get; set; }
        
        [StringLength(100)]
        public string Category { get; set; }
        
        public DateTime DateTaken { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

    public class VisitorFeedback
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Email { get; set; }
        
        [StringLength(100)]
        public string Subject { get; set; }
        
        [Required]
        [StringLength(2000)]
        public string Message { get; set; }
        
        [Range(1, 5)]
        public int? Rating { get; set; }
        
        [StringLength(50)]
        public string IpAddress { get; set; }
        
        [StringLength(500)]
        public string UserAgent { get; set; }
        
        public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        public bool IsActive { get; set; } = true;
        
        // AI Summarization
        [StringLength(1000)]
        public string AISummary { get; set; }
        
        [StringLength(100)]
        public string Sentiment { get; set; }
    }

    // AI-Powered Features Models
    public class JobDescription
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string JobTitle { get; set; }
        
        [StringLength(200)]
        public string Company { get; set; }
        
        [Required]
        [StringLength(10000)]
        public string Description { get; set; }
        
        [StringLength(2000)]
        public string Requirements { get; set; }
        
        [StringLength(1000)]
        public string ExtractedSkills { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        
        // AI Analysis Results
        [StringLength(5000)]
        public string AIAnalysis { get; set; }
        
        public virtual ICollection<AISkillHighlight> AISkillHighlights { get; set; } = new List<AISkillHighlight>();
    }

    public class AISkillHighlight
    {
        public int Id { get; set; }
        
        public int JobDescriptionId { get; set; }
        public virtual JobDescription JobDescription { get; set; }
        
        public int SkillId { get; set; }
        public virtual Skill Skill { get; set; }
        
        [Range(0, 100)]
        public int RelevanceScore { get; set; }
        
        [StringLength(500)]
        public string HighlightReason { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

    public class ProjectRanking
    {
        public int Id { get; set; }
        
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        
        public int JobDescriptionId { get; set; }
        public virtual JobDescription JobDescription { get; set; }
        
        [Range(0, 100)]
        public int RelevanceScore { get; set; }
        
        [StringLength(1000)]
        public string RankingReason { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
