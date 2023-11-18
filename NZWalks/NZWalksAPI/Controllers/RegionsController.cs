using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NzWalksDbContext _dbContext;
        private readonly IRegionRepositories _regionRepositories;

        public RegionsController(NzWalksDbContext dbContext, IRegionRepositories regionRepositories)
        {
            _dbContext = dbContext;
            _regionRepositories = regionRepositories;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await _regionRepositories.GetAllAsync();
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await _regionRepositories.GetByIdAsync(id);
            if(region is null)
            {
              return NotFound();
            }

            var regionDto = new RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
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

            regionDomainModel = await _regionRepositories.CreateAsync(regionDomainModel);

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
            var region = new Region
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                RegionImageUrl = updateRegionRequest.RegionImageUrl
            };

            var result = await _regionRepositories.UpdateAsync(id, region);
            if(result is null)
            {
                return NotFound();
            }
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
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var recordToDelete = await _regionRepositories.DeleteAsync(id);
            if(recordToDelete == null)
            {
                return NotFound();
            }

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
