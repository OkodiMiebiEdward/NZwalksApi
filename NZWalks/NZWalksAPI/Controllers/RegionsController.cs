using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using System.Reflection.Metadata.Ecma335;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NzWalksDbContext _dbContext;

        public RegionsController(NzWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await _dbContext.Regions.ToListAsync();
            var regionDto = new List<RegionDto>();
            foreach (var region in regionsDomain)
            {
                regionDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                });
            }
            return Ok(regionDto);
           
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await _dbContext.Regions.FindAsync(id);
            if(region is null)
            {
              return NotFound();
            }

            var regionDto = new RegionDto()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            };
            return Ok(regionDto);
        }

        //Post Action: To save regions to the Database
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto newRegion)
        {
            Region regionDomainModel = new Region
            {
                Code = newRegion.Code,
                Name = newRegion.Name,
                RegionImageUrl = newRegion.RegionImageUrl
            };

            await _dbContext.Regions.AddAsync(regionDomainModel); 
            await _dbContext.SaveChangesAsync();

            //Map domain model back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id}, regionDto);
        }

        //Update a Region
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateRegionRequestDto updateRegionRequest)
        {
            var result = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(result is null)
            {
              return NotFound();
            }

            result.Code = updateRegionRequest.Code;
            result.Name = updateRegionRequest.Name;
            result.RegionImageUrl = updateRegionRequest.RegionImageUrl;

            await _dbContext.SaveChangesAsync();

            //convert Domain model to DTO

            var regionDto = new RegionDto
            { 
                Id = result.Id,
                Code = result.Code,
                Name = result.Name,
                RegionImageUrl = result.RegionImageUrl
            };
            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("id:Guid")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var recordToDelete = await _dbContext.Regions.FindAsync(id);
            if(recordToDelete == null)
            {
                return NotFound();
            }
             _dbContext.Regions.Remove(recordToDelete);
            await _dbContext.SaveChangesAsync();

            //Show the deleted record
            var mapRegionDTO = new RegionDto
            {
                Id = recordToDelete.Id,
                Code = recordToDelete.Code,
                Name = recordToDelete.Name,
                RegionImageUrl = recordToDelete.RegionImageUrl
            };
            return Ok(mapRegionDTO);
        }
    }
}
