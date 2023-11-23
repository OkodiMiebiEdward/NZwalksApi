using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Repositories
{
    public class SqlWalkRepository : IWalkRepositories
    {
        private readonly NzWalksDbContext _dbContext;

        public SqlWalkRepository(NzWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
           await _dbContext.Walks.AddAsync(walk);
           await _dbContext.SaveChangesAsync();
           return walk;
        }
    }
}
