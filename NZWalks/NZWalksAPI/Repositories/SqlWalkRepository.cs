using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walkDomainModel = await _dbContext.Walks.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(walkDomainModel is null)
            {
              return null;
            }
            _dbContext.Walks.Remove(walkDomainModel);
            await _dbContext.SaveChangesAsync();
            return walkDomainModel;
        }

        public async Task<List<Walk>> GetAsync()
        {
            var walks = await _dbContext
            .Walks.Include("Difficulty")
            .Include("Region").ToListAsync();

            return walks;
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var walk = await _dbContext.Walks.Include("Difficulty")
               .Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            return walk;
        }

        public async Task<Walk?> UpdateAsync(Guid id,Walk walk)
        {
            var walkDomainModel = await _dbContext.Walks
                .FirstOrDefaultAsync(x => x.Id == id);
            if (walkDomainModel == null)
            {
                return null;
            }
            else
            {
                walkDomainModel.Name = walk.Name;
                walkDomainModel.Description = walk.Description;
                walkDomainModel.LengthInKm = walk.LengthInKm;
                walkDomainModel.WalkImageUrl = walk.WalkImageUrl;
                walkDomainModel.DifficultyId = walk.DifficultyId;
                walkDomainModel.RegionId = walk.RegionId;
                await _dbContext.SaveChangesAsync();
                return walkDomainModel;
            }
        }
    }
}
