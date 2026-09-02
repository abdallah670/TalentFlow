using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Contracts.Infra  
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateRefreshTokenAsync(User user);

        Task<User?> ValidateRefreshTokenAsync(string refreshToken);

        Task RevokeRefreshTokenAsync(string refreshToken);
        Task RevokeAllRefreshTokensForUserAsync(Guid userId);
    }
}
