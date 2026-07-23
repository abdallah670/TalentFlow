using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Domain.Entities.IdentityModule;
using static TalentFlow.Infrastructure.Service.JWTSErvice;

namespace TalentFlow.Infrastructure.Service
{
    public class JWTSErvice : IJWTService
    {

        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;

        public JWTSErvice(
            UserManager<User> userManager,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }



        public async Task<JwtSecurityToken> CreateJwtToken(User user, IList<string> roles)
        {
            var claims = new List<Claim>
    {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())    ,    
            new(JwtRegisteredClaimNames.Email, user.Email!),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("TenantId", user.TenantId.ToString())

    };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: creds);
        }

    }
}
