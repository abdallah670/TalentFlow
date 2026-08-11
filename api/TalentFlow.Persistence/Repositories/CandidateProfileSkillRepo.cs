using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.CandidateModule;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Persistence.Repositories
{
    public class CandidateProfileSkillRepo : GenericRepository<CandidateProfileSkill>, ICandidateProfileSkillRepo
    {
        public CandidateProfileSkillRepo(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
