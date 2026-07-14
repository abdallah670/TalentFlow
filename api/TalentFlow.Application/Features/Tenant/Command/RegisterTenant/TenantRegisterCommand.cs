using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Tenant.Command.RegisterTenant
{
    public class TenantRegisterCommand: IRequest<AuthResponse>
    {
        public string TentantName { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string Slug { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
