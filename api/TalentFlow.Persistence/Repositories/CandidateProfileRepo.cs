using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.CandidateModule;

namespace TalentFlow.Persistence.Repositories
{
    public class CandidateProfileRepo : GenericRepository<CandidateProfile>, IcandidateProfileRepo
    {
        private readonly AppDbContext _dbContext;

        public CandidateProfileRepo(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CandidateProfile?> GetByUserIdWithSkillsAsync(Guid userId)
        {
            return await _dbContext.CandidateProfiles
                .Include(x => x.Skills)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}