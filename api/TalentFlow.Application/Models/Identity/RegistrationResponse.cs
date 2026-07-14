    namespace TalentFlow.Application.Models.Identity
    {
        public class RegistrationResponse
        {
            public string UserId { get; set; } = string.Empty;
            public string? Token { get; set; }
            public string? RefreshToken { get; set; }
            public string Email { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public bool RequiresEmailVerification { get; set; }
        }
    }
