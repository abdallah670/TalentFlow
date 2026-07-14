using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateRefreshTokenAsync(User user);

        Task<User?> ValidateRefreshTokenAsync(string refreshToken);

        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
