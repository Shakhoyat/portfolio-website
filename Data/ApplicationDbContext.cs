using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Models;

namespace PortfolioWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Portfolio Data
        public DbSet<Education> Educations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Dataset> Datasets { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<VisitorFeedback> VisitorFeedbacks { get; set; }
        public DbSet<PersonalInfo> PersonalInfos { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<PhotoGallery> PhotoGalleries { get; set; }
        
        // AI Features
        public DbSet<JobDescription> JobDescriptions { get; set; }
        public DbSet<AISkillHighlight> AISkillHighlights { get; set; }
        public DbSet<ProjectRanking> ProjectRankings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            modelBuilder.Entity<Project>()
                .HasMany(p => p.ProjectRankings)
                .WithOne(pr => pr.Project)
                .HasForeignKey(pr => pr.ProjectId);

            modelBuilder.Entity<JobDescription>()
                .HasMany(jd => jd.AISkillHighlights)
                .WithOne(ash => ash.JobDescription)
                .HasForeignKey(ash => ash.JobDescriptionId);

            // Configure indexes for better performance
            modelBuilder.Entity<Publication>()
                .HasIndex(p => p.PublicationDate);

            modelBuilder.Entity<Project>()
                .HasIndex(p => p.CreatedDate);

            modelBuilder.Entity<VisitorFeedback>()
                .HasIndex(vf => vf.SubmittedDate);
        }
    }
}
