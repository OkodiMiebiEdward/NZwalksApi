using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Migrations;
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

        
        public async Task<List<Walk>> GetAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null
            , bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = _dbContext.Walks
                .Include("Difficulty")
                .Include("Region").AsQueryable();

            //Fitering
            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if(filterOn.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                   walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //Sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                  walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x =>  x.Name);
                }
                else if(sortBy.Equals("Length",StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //Pagination
            var skipResult = (pageNumber - 1) * pageSize;
            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
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
