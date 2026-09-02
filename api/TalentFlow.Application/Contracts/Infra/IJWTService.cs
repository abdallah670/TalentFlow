using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Contracts.Infra  
{
    public interface IJWTService
    {
       Task< JwtSecurityToken> CreateJwtToken(User user, IList<string> roles);
    }
}
