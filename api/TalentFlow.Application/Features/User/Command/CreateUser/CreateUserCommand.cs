using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.User.Command.CreateUser
{
    public class CreateUserCommand :IRequest<BaseCommandResponse>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Role { get; set; } = default!;
    
}
}
