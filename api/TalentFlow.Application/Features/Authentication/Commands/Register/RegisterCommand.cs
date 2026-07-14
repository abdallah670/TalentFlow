using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Features.Authontication.DTOs;

namespace TalentFlow.Application.Features.Authontication.Commands.Register
{
    public class RegisterCommand :IRequest<AuthResponse>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string UserName { get; set; } = default!;

        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string CompanyName { get; set; } = default!;
    }
}
