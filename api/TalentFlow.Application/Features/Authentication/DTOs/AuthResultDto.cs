using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Authontication.DTOs
{
    public class AuthResultDto
    {
        public bool IsAuthenticated { get; set; }
        public string? Message { get; set; }

        public Guid UserId { get; set; }
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        public Guid? TenantId { get; set; }
        public List<string> Roles { get; set; } = new();

        public string Token { get; set; } = default!;
        public DateTime TokenExpiration { get; set; }

        public string RefreshToken { get; set; } = default!;
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
