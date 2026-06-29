namespace TalentFlow.Application.Models
{
    public class AISettings
    {
        public const string SectionName = "AI";
        
        public string Provider { get; set; } = "Gemini";  // "Gemini" | "OpenAI"
        public string ApiKey { get; set; } = string.Empty;
        public string Endpoint { get; set; } = "https://generativelanguage.googleapis.com/v1beta/models/";
        public string ModelName { get; set; } = "gemini-1.5-flash";
        public int MaxTokens { get; set; } = 1024;
        public double Temperature { get; set; } = 0.7;
        public int MaxHistoryMessages { get; set; } = 10;
        public int CacheMinutes { get; set; } = 30;
    }
}