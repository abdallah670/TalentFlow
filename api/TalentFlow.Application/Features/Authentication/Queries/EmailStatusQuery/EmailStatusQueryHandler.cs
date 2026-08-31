using MediatR;
using Microsoft.AspNetCore.Identity;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Authentication.Queries.EmailStatus
{
    public class EmailStatusQueryHandler : IRequestHandler<EmailStatusQuery, EmailStatusResponse>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;

        public EmailStatusQueryHandler(UserManager<Domain.Entities.IdentityModule.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<EmailStatusResponse> Handle(EmailStatusQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            return new EmailStatusResponse
            {
                IsRegistered = user is not null,
                IsConfirmed = user?.EmailConfirmed ?? false
            };
        }
    }
}