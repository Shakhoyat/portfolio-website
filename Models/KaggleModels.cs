using System.ComponentModel.DataAnnotations;

namespace PortfolioWebsite.Models
{
    public class KaggleProfile
    {
        public string Username { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Bio { get; set; } = "";
        public string Location { get; set; } = "";
        public DateTime JoinDate { get; set; }
        public string ProfileImageUrl { get; set; } = "";
        public string CompetitionRank { get; set; } = "";
        public string DatasetRank { get; set; } = "";
        public string NotebookRank { get; set; } = "";
        public string DiscussionRank { get; set; } = "";
        public KaggleMedals CompetitionMedals { get; set; } = new();
        public KaggleMedals DatasetMedals { get; set; } = new();
        public KaggleMedals NotebookMedals { get; set; } = new();
        public KaggleMedals DiscussionMedals { get; set; } = new();
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public DateTime LastActive { get; set; }
    }

    public class KaggleMedals
    {
        public int Gold { get; set; }
        public int Silver { get; set; }
        public int Bronze { get; set; }
        
        public int Total => Gold + Silver + Bronze;
    }

    public class KaggleCompetition
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int Rank { get; set; }
        public int TotalTeams { get; set; }
        public double Score { get; set; }
        public string Medal { get; set; } = "";
        public string CompetitionUrl { get; set; } = "";
        public DateTime SubmissionDate { get; set; }
        public string Category { get; set; } = "";
        
        public string PercentileRank => TotalTeams > 0 ? $"Top {Math.Round((double)Rank / TotalTeams * 100, 1)}%" : "";
        public string MedalIcon => Medal.ToLower() switch
        {
            "gold" => "fas fa-medal text-yellow-500",
            "silver" => "fas fa-medal text-gray-400", 
            "bronze" => "fas fa-medal text-orange-600",
            _ => "fas fa-trophy text-blue-500"
        };
        public string MedalColor => Medal.ToLower() switch
        {
            "gold" => "bg-yellow-100 text-yellow-800 border-yellow-300",
            "silver" => "bg-gray-100 text-gray-800 border-gray-300",
            "bronze" => "bg-orange-100 text-orange-800 border-orange-300",
            _ => "bg-blue-100 text-blue-800 border-blue-300"
        };
    }

    public class KaggleDataset
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int DownloadCount { get; set; }
        public int ViewCount { get; set; }
        public int VoteCount { get; set; }
        public string Medal { get; set; } = "";
        public string DatasetUrl { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Size { get; set; } = "";
        
        public string MedalIcon => Medal.ToLower() switch
        {
            "gold" => "fas fa-medal text-yellow-500",
            "silver" => "fas fa-medal text-gray-400",
            "bronze" => "fas fa-medal text-orange-600",
            _ => "fas fa-database text-blue-500"
        };
    }

    public class KaggleRankings
    {
        public KaggleRanking CompetitionRanking { get; set; } = new();
        public KaggleRanking DatasetRanking { get; set; } = new();
        public KaggleRanking NotebookRanking { get; set; } = new();
        public KaggleRanking DiscussionRanking { get; set; } = new();
    }

    public class KaggleRanking
    {
        public int CurrentRank { get; set; }
        public int HighestRank { get; set; }
        public int Points { get; set; }
        public string Tier { get; set; } = "";
        
        public string TierIcon => Tier.ToLower() switch
        {
            "grandmaster" => "fas fa-crown text-purple-600",
            "master" => "fas fa-star text-yellow-500",
            "expert" => "fas fa-gem text-blue-500",
            "contributor" => "fas fa-user-check text-green-500",
            "novice" => "fas fa-seedling text-gray-500",
            _ => "fas fa-user text-gray-400"
        };
        
        public string TierColor => Tier.ToLower() switch
        {
            "grandmaster" => "text-purple-600 bg-purple-100 border-purple-300",
            "master" => "text-yellow-600 bg-yellow-100 border-yellow-300",
            "expert" => "text-blue-600 bg-blue-100 border-blue-300",
            "contributor" => "text-green-600 bg-green-100 border-green-300",
            "novice" => "text-gray-600 bg-gray-100 border-gray-300",
            _ => "text-gray-500 bg-gray-50 border-gray-200"
        };
    }

    // Database entities for storing Kaggle data
    public class KaggleAchievement
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; } = "";
        
        public string Description { get; set; } = "";
        
        [Required]
        public string Type { get; set; } = ""; // Competition, Dataset, Notebook, Discussion
        
        public string Medal { get; set; } = ""; // Gold, Silver, Bronze
        
        public int? Rank { get; set; }
        public int? TotalParticipants { get; set; }
        public double? Score { get; set; }
        
        public string ExternalUrl { get; set; } = "";
        public DateTime AchievedDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; }
        
        public string Category { get; set; } = "";
        public string Tags { get; set; } = ""; // JSON array of tags
        
        // Computed properties
        public string PercentileRank => TotalParticipants.HasValue && Rank.HasValue && TotalParticipants > 0 
            ? $"Top {Math.Round((double)Rank.Value / TotalParticipants.Value * 100, 1)}%" 
            : "";
            
        public string MedalIcon => Medal.ToLower() switch
        {
            "gold" => "fas fa-medal text-yellow-500",
            "silver" => "fas fa-medal text-gray-400",
            "bronze" => "fas fa-medal text-orange-600",
            _ => GetTypeIcon()
        };
        
        private string GetTypeIcon() => Type.ToLower() switch
        {
            "competition" => "fas fa-trophy text-blue-500",
            "dataset" => "fas fa-database text-green-500",
            "notebook" => "fas fa-book text-purple-500",
            "discussion" => "fas fa-comments text-orange-500",
            _ => "fas fa-award text-gray-500"
        };
        
        public string TypeIcon => GetTypeIcon();
    }

    public class KaggleSync
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public DateTime LastSyncDate { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; } = "";
        public int NewAchievements { get; set; }
        public int UpdatedAchievements { get; set; }
    }
}
