using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Contracts.Infra  
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);

    }
}
