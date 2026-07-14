using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Domain.Entities.IdentityModule;
using TalentFlow.Persistence;

namespace TalentFlow.Infrastructure.Service
{
    public class RefreshTokenService :IRefreshTokenService
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;
        public RefreshTokenService(
                AppDbContext context,
                IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> GenerateRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshTokenString();

            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = HashToken(refreshToken),
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays)
            };

            _context.RefreshTokens.Add(refreshTokenEntity);

            await _context.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<User?> ValidateRefreshTokenAsync(string refreshToken)
        {
            var tokenHash = HashToken(refreshToken);

            var storedToken = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

            if (storedToken is null)
                return null;

            if (!storedToken.IsActive)
                return null;

            return storedToken.User;
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var tokenHash = HashToken(refreshToken);

            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

            if (storedToken is null)
                return;

            storedToken.RevokedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
        public string GenerateRefreshTokenString()
            {
                var bytes = new byte[64];

                RandomNumberGenerator.Fill(bytes);

                return Convert.ToBase64String(bytes);
            }

            public string HashToken(string token)
            {
                var bytes = Encoding.UTF8.GetBytes(token);

                var hash = SHA256.HashData(bytes);

                return Convert.ToBase64String(hash);
            }
        }
    }

