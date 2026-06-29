using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Persistence.Repositories;

public class CandidateDocumentRepository : GenericRepository<CandidateDocument>, ICandidateDocumentRepository
{
    public CandidateDocumentRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
