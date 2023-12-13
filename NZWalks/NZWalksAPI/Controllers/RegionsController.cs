using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepositories _regionRepositories;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(IRegionRepositories regionRepositories, IMapper mapper, ILogger<RegionsController> logger)
        {
            _regionRepositories = regionRepositories;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
       //[Authorize(Roles = "READER")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //throw new Exception("This is a custom exception");
                var regionsDomain = await _regionRepositories.GetAllAsync();
                //Using Automapper to map data.
                var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);
                _logger.LogInformation($"Finished get all region requests with data:{JsonSerializer.Serialize(regionsDomain)}");
                return Ok(regionsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "READER")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await _regionRepositories.GetByIdAsync(id);
            if(region is null)
            {
              return NotFound();
            }

            //var regionDto = new RegionDto
            //{
            //    Id = region.Id,
            //    Code = region.Code,
            //    Name = region.Name,
            //    RegionImageUrl = region.RegionImageUrl
            //};
            var regionDto = _mapper.Map<RegionDto>(region);
            return Ok(regionDto);
        }

        //Post Action: To save regions to the Database
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "WRITER")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto newRegion)
        { 
            
                var regionDomainModel = _mapper.Map<Region>(newRegion);
                regionDomainModel = await _regionRepositories.CreateAsync(regionDomainModel);
                //Map domain model back to DTO
                var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        //Update a Region
        [HttpPut]
        [ValidateModel]
        //[Authorize(Roles = "WRITER")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateRegionRequestDto updateRegionRequest)
        { 
                var region = _mapper.Map<Region>(updateRegionRequest);

                var result = await _regionRepositories.UpdateAsync(id, region);
                if (result is null)
                {
                    return NotFound();
                }
                //convert Domain model to DTO
                var regionDto = _mapper.Map<RegionDto>(result);
                return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "WRITER")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var recordToDelete = await _regionRepositories.DeleteAsync(id);
            if(recordToDelete == null)
            {
                return NotFound();
            }

            //Show the deleted record
            var mapRegionDTO = _mapper.Map<RegionDto>(recordToDelete);
            return Ok(mapRegionDTO);
        }
    }
}
