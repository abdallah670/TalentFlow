using System.ComponentModel.DataAnnotations;

namespace TalentFlow.Application.Models.Identity
{
    public class AuthRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
