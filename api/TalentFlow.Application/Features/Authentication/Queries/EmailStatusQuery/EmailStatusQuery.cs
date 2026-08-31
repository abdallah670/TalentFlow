using MediatR;

namespace TalentFlow.Application.Features.Authentication.Queries.EmailStatus
{
    public class EmailStatusQuery : IRequest<EmailStatusResponse>
    {
        public string Email { get; set; } = string.Empty;
    }

    public class EmailStatusResponse
    {
        public bool IsRegistered { get; set; }
        public bool IsConfirmed { get; set; }
    }
}