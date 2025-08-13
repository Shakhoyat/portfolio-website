#nullable disable
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PortfolioWebsite.Data;
using PortfolioWebsite.Models;
using RestSharp;

namespace PortfolioWebsite.Services
{
    public class GeminiAIService : IGeminiAIService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GeminiAIService(ApplicationDbContext context, IConfiguration configuration, HttpClient httpClient)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<string> AnalyzeJobDescriptionAsync(string jobDescription)
        {
            try
            {
                var apiKey = _configuration["GeminiAI:ApiKey"];
                var prompt = $@"
                    Analyze the following job description and extract:
                    1. Key skills required
                    2. Experience level needed
                    3. Technologies mentioned
                    4. Industry focus
                    5. Role responsibilities
                    
                    Job Description:
                    {jobDescription}
                    
                    Provide a structured analysis in JSON format.
                ";

                // Call Gemini API (placeholder implementation)
                var response = await CallGeminiAPIAsync(prompt, apiKey);
                return response;
            }
            catch (Exception ex)
            {
                // Log error and return fallback analysis
                return GenerateFallbackAnalysis(jobDescription);
            }
        }

        public async Task<IEnumerable<AISkillHighlight>> GenerateSkillHighlightsAsync(int jobDescriptionId)
        {
            var jobDescription = await _context.JobDescriptions.FindAsync(jobDescriptionId);
            if (jobDescription == null) return new List<AISkillHighlight>();

            var skills = await _context.Skills.Where(s => s.IsActive).ToListAsync();
            var highlights = new List<AISkillHighlight>();

            var extractedSkills = ExtractSkillsFromText(jobDescription.Description);

            foreach (var skill in skills)
            {
                var relevanceScore = CalculateSkillRelevance(skill, extractedSkills, jobDescription.Description);
                if (relevanceScore > 30) // Only include relevant skills
                {
                    highlights.Add(new AISkillHighlight
                    {
                        JobDescriptionId = jobDescriptionId,
                        SkillId = skill.Id,
                        RelevanceScore = relevanceScore,
                        HighlightReason = GenerateHighlightReason(skill, relevanceScore),
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }

            return highlights.OrderByDescending(h => h.RelevanceScore);
        }

        public async Task<IEnumerable<ProjectRanking>> RankProjectsForJobAsync(int jobDescriptionId)
        {
            var jobDescription = await _context.JobDescriptions.FindAsync(jobDescriptionId);
            if (jobDescription == null) return new List<ProjectRanking>();

            var projects = await _context.Projects.Where(p => p.IsActive).ToListAsync();
            var rankings = new List<ProjectRanking>();

            foreach (var project in projects)
            {
                var relevanceScore = CalculateProjectRelevance(project, jobDescription);
                rankings.Add(new ProjectRanking
                {
                    ProjectId = project.Id,
                    JobDescriptionId = jobDescriptionId,
                    RelevanceScore = relevanceScore,
                    RankingReason = GenerateRankingReason(project, relevanceScore),
                    CreatedDate = DateTime.UtcNow
                });
            }

            return rankings.OrderByDescending(r => r.RelevanceScore);
        }

        public async Task<string> SummarizeFeedbackAsync(string feedback)
        {
            try
            {
                var apiKey = _configuration["GeminiAI:ApiKey"];
                var prompt = $@"
                    Summarize the following visitor feedback in 2-3 sentences, focusing on key points and sentiment:
                    
                    Feedback:
                    {feedback}
                    
                    Provide a concise summary.
                ";

                var response = await CallGeminiAPIAsync(prompt, apiKey);
                return response;
            }
            catch (Exception ex)
            {
                // Fallback summarization
                return GenerateFallbackSummary(feedback);
            }
        }

        public async Task<string> AnalyzeFeedbackSentimentAsync(string feedback)
        {
            try
            {
                var apiKey = _configuration["GeminiAI:ApiKey"];
                var prompt = $@"
                    Analyze the sentiment of the following feedback and classify it as:
                    - Positive
                    - Negative
                    - Neutral
                    - Mixed
                    
                    Feedback:
                    {feedback}
                    
                    Return only the sentiment classification.
                ";

                var response = await CallGeminiAPIAsync(prompt, apiKey);
                return response.Trim();
            }
            catch (Exception ex)
            {
                // Fallback sentiment analysis
                return AnalyzeSentimentFallback(feedback);
            }
        }

        private async Task<string> CallGeminiAPIAsync(string prompt, string apiKey)
        {
            // Placeholder for actual Gemini API call
            // In production, implement the actual Gemini API integration
            
            var client = new RestClient("https://generativelanguage.googleapis.com");
            var request = new RestRequest($"/v1beta/models/gemini-pro:generateContent?key={apiKey}", Method.Post);
            
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            request.AddJsonBody(requestBody);
            request.AddHeader("Content-Type", "application/json");

            var response = await client.ExecuteAsync(request);
            
            if (response.IsSuccessful)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return result?.candidates?[0]?.content?.parts?[0]?.text ?? "No response generated";
            }

            throw new Exception($"API call failed: {response.ErrorMessage}");
        }

        private string GenerateFallbackAnalysis(string jobDescription)
        {
            // Simple keyword-based analysis as fallback
            var analysis = new
            {
                KeySkills = ExtractKeywords(jobDescription, new[] { "skill", "experience", "knowledge", "proficient" }),
                Technologies = ExtractKeywords(jobDescription, new[] { "python", "javascript", "react", "angular", "sql", "aws", "azure" }),
                ExperienceLevel = DetermineExperienceLevel(jobDescription),
                Industry = DetermineIndustry(jobDescription)
            };

            return JsonConvert.SerializeObject(analysis, Formatting.Indented);
        }

        private List<string> ExtractSkillsFromText(string text)
        {
            var commonSkills = new[]
            {
                "Python", "JavaScript", "C#", "Java", "SQL", "React", "Angular", "Vue.js",
                "Machine Learning", "Data Science", "AI", "AWS", "Azure", "Docker", "Kubernetes",
                "Git", "Agile", "Scrum", "REST API", "GraphQL", "MongoDB", "PostgreSQL"
            };

            return commonSkills.Where(skill => 
                text.Contains(skill, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        private int CalculateSkillRelevance(Skill skill, List<string> extractedSkills, string jobDescription)
        {
            int score = 0;

            // Direct skill name match
            if (extractedSkills.Any(s => s.Equals(skill.Name, StringComparison.OrdinalIgnoreCase)))
            {
                score += 50;
            }

            // Partial match in job description
            if (jobDescription.Contains(skill.Name, StringComparison.OrdinalIgnoreCase))
            {
                score += 30;
            }

            // Category relevance
            if (skill.Category != null && jobDescription.Contains(skill.Category, StringComparison.OrdinalIgnoreCase))
            {
                score += 20;
            }

            // Proficiency level bonus
            score += skill.ProficiencyLevel * 2;

            return Math.Min(score, 100);
        }

        private int CalculateProjectRelevance(Project project, JobDescription jobDescription)
        {
            int score = 0;

            // Technology stack match
            if (project.TechnologiesUsed != null)
            {
                var projectTechs = project.TechnologiesUsed.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var tech in projectTechs)
                {
                    if (jobDescription.Description.Contains(tech.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        score += 20;
                    }
                }
            }

            // Category relevance
            var categoryKeywords = GetCategoryKeywords(project.Category);
            foreach (var keyword in categoryKeywords)
            {
                if (jobDescription.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    score += 15;
                }
            }

            // Project complexity
            score += project.DifficultyLevel * 5;

            // Recent projects get bonus
            if (project.CreatedDate > DateTime.UtcNow.AddYears(-2))
            {
                score += 10;
            }

            return Math.Min(score, 100);
        }

        private string[] GetCategoryKeywords(ProjectCategory category)
        {
            return category switch
            {
                ProjectCategory.MachineLearning => new[] { "machine learning", "ML", "AI", "neural network", "deep learning" },
                ProjectCategory.DataScience => new[] { "data science", "analytics", "visualization", "statistics" },
                ProjectCategory.WebDevelopment => new[] { "web", "frontend", "backend", "full stack", "HTML", "CSS" },
                ProjectCategory.MobileApp => new[] { "mobile", "app", "iOS", "Android", "React Native", "Flutter" },
                ProjectCategory.NLP => new[] { "NLP", "natural language", "text processing", "chatbot" },
                ProjectCategory.ComputerVision => new[] { "computer vision", "image processing", "OpenCV", "CNN" },
                _ => new string[0]
            };
        }

        private string GenerateHighlightReason(Skill skill, int relevanceScore)
        {
            if (relevanceScore > 80)
                return "Highly relevant skill mentioned prominently in job requirements";
            if (relevanceScore > 60)
                return "Relevant skill that matches job description";
            if (relevanceScore > 40)
                return "Potentially relevant skill based on category match";
            return "Skill may be indirectly relevant";
        }

        private string GenerateRankingReason(Project project, int relevanceScore)
        {
            if (relevanceScore > 80)
                return "Excellent match - technologies and domain align perfectly";
            if (relevanceScore > 60)
                return "Good match - several relevant technologies used";
            if (relevanceScore > 40)
                return "Moderate match - some relevant aspects";
            return "Limited relevance but demonstrates general capabilities";
        }

        private string GenerateFallbackSummary(string feedback)
        {
            var sentences = feedback.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (sentences.Length <= 2)
                return feedback;

            return string.Join(". ", sentences.Take(2)) + ".";
        }

        private string AnalyzeSentimentFallback(string feedback)
        {
            var positiveWords = new[] { "good", "great", "excellent", "amazing", "love", "like", "awesome", "fantastic" };
            var negativeWords = new[] { "bad", "terrible", "awful", "hate", "dislike", "poor", "horrible" };

            var text = feedback.ToLowerInvariant();
            var positiveCount = positiveWords.Count(word => text.Contains(word));
            var negativeCount = negativeWords.Count(word => text.Contains(word));

            if (positiveCount > negativeCount) return "Positive";
            if (negativeCount > positiveCount) return "Negative";
            if (positiveCount > 0 && negativeCount > 0) return "Mixed";
            return "Neutral";
        }

        private List<string> ExtractKeywords(string text, string[] seedWords)
        {
            var words = text.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return seedWords.Where(seed => words.Any(word => word.Contains(seed))).ToList();
        }

        private string DetermineExperienceLevel(string jobDescription)
        {
            var text = jobDescription.ToLowerInvariant();
            if (text.Contains("senior") || text.Contains("lead") || text.Contains("5+ years"))
                return "Senior";
            if (text.Contains("junior") || text.Contains("entry") || text.Contains("1-2 years"))
                return "Junior";
            return "Mid-level";
        }

        private string DetermineIndustry(string jobDescription)
        {
            var text = jobDescription.ToLowerInvariant();
            if (text.Contains("fintech") || text.Contains("banking")) return "Finance";
            if (text.Contains("healthcare") || text.Contains("medical")) return "Healthcare";
            if (text.Contains("ecommerce") || text.Contains("retail")) return "E-commerce";
            if (text.Contains("startup")) return "Startup";
            return "Technology";
        }
    }
}
