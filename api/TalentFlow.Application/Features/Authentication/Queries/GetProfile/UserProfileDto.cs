namespace TalentFlow.Application.Features.Authentication.Queries.GetProfile
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new();
    }
}