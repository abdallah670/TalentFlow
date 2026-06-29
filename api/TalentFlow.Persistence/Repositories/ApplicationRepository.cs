using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Persistence.Repositories;

public class ApplicationRepository : GenericRepository<TalentFlow.Domain.Entities.RecruitmentModule.Application>, IApplicationRepository
{
    public ApplicationRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
