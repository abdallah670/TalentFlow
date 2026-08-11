using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.CandidateModule;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Persistence.Repositories
{
   
        public class InvitationRepository : GenericRepository<Invitation>, IInvitationRepository
        {
            public InvitationRepository(AppDbContext dbContext) : base(dbContext)
            {
            }
        }

    
}
