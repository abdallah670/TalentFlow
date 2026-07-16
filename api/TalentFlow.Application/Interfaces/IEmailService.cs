using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);

    }
}
