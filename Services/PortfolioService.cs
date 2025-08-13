#nullable disable
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Data;
using PortfolioWebsite.Models;

namespace PortfolioWebsite.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly ApplicationDbContext _context;

        public PortfolioService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Personal Info
        public async Task<PersonalInfo> GetPersonalInfoAsync()
        {
            return await _context.PersonalInfos.FirstOrDefaultAsync() ?? new PersonalInfo();
        }

        public async Task UpdatePersonalInfoAsync(PersonalInfo personalInfo)
        {
            var existing = await _context.PersonalInfos.FirstOrDefaultAsync();
            if (existing == null)
            {
                personalInfo.LastUpdated = DateTime.UtcNow;
                _context.PersonalInfos.Add(personalInfo);
            }
            else
            {
                existing.FullName = personalInfo.FullName;
                existing.Title = personalInfo.Title;
                existing.Bio = personalInfo.Bio;
                existing.AboutMe = personalInfo.AboutMe;
                existing.Email = personalInfo.Email;
                existing.Phone = personalInfo.Phone;
                existing.Location = personalInfo.Location;
                existing.ProfileImageUrl = personalInfo.ProfileImageUrl;
                existing.ResumeUrl = personalInfo.ResumeUrl;
                existing.LastUpdated = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }

        // Education
        public async Task<IEnumerable<Education>> GetEducationsAsync()
        {
            return await _context.Educations
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();
        }

        public async Task<Education> GetEducationByIdAsync(int id)
        {
            return await _context.Educations.FindAsync(id);
        }

        public async Task CreateEducationAsync(Education education)
        {
            education.CreatedDate = DateTime.UtcNow;
            _context.Educations.Add(education);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEducationAsync(Education education)
        {
            _context.Educations.Update(education);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEducationAsync(int id)
        {
            var education = await _context.Educations.FindAsync(id);
            if (education != null)
            {
                education.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        // Experience
        public async Task<IEnumerable<Experience>> GetExperiencesAsync()
        {
            return await _context.Experiences
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();
        }

        public async Task<Experience> GetExperienceByIdAsync(int id)
        {
            return await _context.Experiences.FindAsync(id);
        }

        public async Task CreateExperienceAsync(Experience experience)
        {
            experience.CreatedDate = DateTime.UtcNow;
            _context.Experiences.Add(experience);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExperienceAsync(Experience experience)
        {
            _context.Experiences.Update(experience);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExperienceAsync(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience != null)
            {
                experience.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        // Skills
        public async Task<IEnumerable<Skill>> GetSkillsAsync()
        {
            return await _context.Skills
                .Where(s => s.IsActive)
                .OrderBy(s => s.Category)
                .ThenByDescending(s => s.ProficiencyLevel)
                .ToListAsync();
        }

        public async Task<IEnumerable<Skill>> GetSkillsByCategoryAsync(string category)
        {
            return await _context.Skills
                .Where(s => s.IsActive && s.Category == category)
                .OrderByDescending(s => s.ProficiencyLevel)
                .ToListAsync();
        }

        public async Task<Skill> GetSkillByIdAsync(int id)
        {
            return await _context.Skills.FindAsync(id);
        }

        public async Task CreateSkillAsync(Skill skill)
        {
            skill.CreatedDate = DateTime.UtcNow;
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSkillAsync(Skill skill)
        {
            _context.Skills.Update(skill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSkillAsync(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill != null)
            {
                skill.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        // Projects
        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await _context.Projects
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetFeaturedProjectsAsync()
        {
            return await _context.Projects
                .Where(p => p.IsActive && p.IsFeatured)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.ProjectRankings)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateProjectAsync(Project project)
        {
            project.CreatedDate = DateTime.UtcNow;
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                project.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        // Publications
        public async Task<IEnumerable<Publication>> GetPublicationsAsync()
        {
            return await _context.Publications
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.PublicationDate)
                .ToListAsync();
        }

        public async Task<Publication> GetPublicationByIdAsync(int id)
        {
            return await _context.Publications.FindAsync(id);
        }

        public async Task CreatePublicationAsync(Publication publication)
        {
            publication.CreatedDate = DateTime.UtcNow;
            _context.Publications.Add(publication);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePublicationAsync(Publication publication)
        {
            _context.Publications.Update(publication);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePublicationAsync(int id)
        {
            var publication = await _context.Publications.FindAsync(id);
            if (publication != null)
            {
                publication.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        // Achievements
        public async Task<IEnumerable<Achievement>> GetAchievementsAsync()
        {
            return await _context.Achievements
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.DateReceived)
                .ToListAsync();
        }

        public async Task<Achievement> GetAchievementByIdAsync(int id)
        {
            return await _context.Achievements.FindAsync(id);
        }

        public async Task CreateAchievementAsync(Achievement achievement)
        {
            achievement.CreatedDate = DateTime.UtcNow;
            _context.Achievements.Add(achievement);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAchievementAsync(Achievement achievement)
        {
            _context.Achievements.Update(achievement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAchievementAsync(int id)
        {
            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement != null)
            {
                achievement.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        // Datasets
        public async Task<IEnumerable<Dataset>> GetDatasetsAsync()
        {
            return await _context.Datasets
                .Where(d => d.IsActive)
                .OrderByDescending(d => d.CreatedDate)
                .ToListAsync();
        }

        public async Task<Dataset> GetDatasetByIdAsync(int id)
        {
            return await _context.Datasets.FindAsync(id);
        }

        public async Task CreateDatasetAsync(Dataset dataset)
        {
            dataset.CreatedDate = DateTime.UtcNow;
            dataset.LastUpdated = DateTime.UtcNow;
            _context.Datasets.Add(dataset);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDatasetAsync(Dataset dataset)
        {
            dataset.LastUpdated = DateTime.UtcNow;
            _context.Datasets.Update(dataset);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDatasetAsync(int id)
        {
            var dataset = await _context.Datasets.FindAsync(id);
            if (dataset != null)
            {
                dataset.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        // Social Links
        public async Task<IEnumerable<SocialLink>> GetSocialLinksAsync()
        {
            return await _context.SocialLinks
                .Where(s => s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();
        }

        public async Task<SocialLink> GetSocialLinkByIdAsync(int id)
        {
            return await _context.SocialLinks.FindAsync(id);
        }

        public async Task CreateSocialLinkAsync(SocialLink socialLink)
        {
            socialLink.CreatedDate = DateTime.UtcNow;
            _context.SocialLinks.Add(socialLink);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSocialLinkAsync(SocialLink socialLink)
        {
            _context.SocialLinks.Update(socialLink);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSocialLinkAsync(int id)
        {
            var socialLink = await _context.SocialLinks.FindAsync(id);
            if (socialLink != null)
            {
                socialLink.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        // Photo Gallery
        public async Task<IEnumerable<PhotoGallery>> GetPhotoGalleryAsync()
        {
            return await _context.PhotoGalleries
                .Where(p => p.IsActive)
                .OrderBy(p => p.DisplayOrder)
                .ThenByDescending(p => p.DateTaken)
                .ToListAsync();
        }

        public async Task<PhotoGallery> GetPhotoByIdAsync(int id)
        {
            return await _context.PhotoGalleries.FindAsync(id);
        }

        public async Task CreatePhotoAsync(PhotoGallery photo)
        {
            photo.CreatedDate = DateTime.UtcNow;
            _context.PhotoGalleries.Add(photo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePhotoAsync(PhotoGallery photo)
        {
            _context.PhotoGalleries.Update(photo);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePhotoAsync(int id)
        {
            var photo = await _context.PhotoGalleries.FindAsync(id);
            if (photo != null)
            {
                photo.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        // Visitor Feedback
        public async Task<IEnumerable<VisitorFeedback>> GetVisitorFeedbackAsync()
        {
            return await _context.VisitorFeedbacks
                .Where(f => f.IsActive)
                .OrderByDescending(f => f.SubmittedDate)
                .ToListAsync();
        }

        public async Task<VisitorFeedback> GetFeedbackByIdAsync(int id)
        {
            return await _context.VisitorFeedbacks.FindAsync(id);
        }

        public async Task CreateFeedbackAsync(VisitorFeedback feedback)
        {
            feedback.SubmittedDate = DateTime.UtcNow;
            _context.VisitorFeedbacks.Add(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFeedbackAsync(VisitorFeedback feedback)
        {
            _context.VisitorFeedbacks.Update(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFeedbackAsync(int id)
        {
            var feedback = await _context.VisitorFeedbacks.FindAsync(id);
            if (feedback != null)
            {
                feedback.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkFeedbackAsReadAsync(int id)
        {
            var feedback = await _context.VisitorFeedbacks.FindAsync(id);
            if (feedback != null)
            {
                feedback.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
