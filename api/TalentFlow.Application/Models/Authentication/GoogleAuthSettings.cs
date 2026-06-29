namespace TalentFlow.Application.Models.Authentication
{
    public class GoogleAuthSettings
    {
        public const string SectionName = "Authentication:Google";
        
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}