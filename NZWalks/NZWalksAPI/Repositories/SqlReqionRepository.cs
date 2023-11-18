using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class SqlReqionRepository : IRegionRepositories
    {
        private readonly NzWalksDbContext _dbContext;
       
        public SqlReqionRepository(NzWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null) 
            { 
                return null;
            }

            _dbContext.Regions.Remove(existingRegion);  
            await _dbContext.SaveChangesAsync();
            return existingRegion;
        }

        public async Task<List<Region>> GetAllAsync()
        {
           var regions = await _dbContext.Regions.ToListAsync();
           return regions;
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
           return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Region?> GetByIdASync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await _dbContext.Regions.FindAsync(id);
            if(existingRegion is null)
            {
                return null;
            };

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await _dbContext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
