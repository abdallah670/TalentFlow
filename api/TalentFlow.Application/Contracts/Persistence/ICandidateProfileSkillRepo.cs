using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Domain.Entities.CandidateModule;

namespace TalentFlow.Application.Contracts.Persistence
{
    public interface ICandidateProfileSkillRepo :IGenericRepository<CandidateProfileSkill> 
    {
    }
}
