using AutoMapper;
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
        
        private readonly IRegionRepositories _regionRepositories;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepositories regionRepositories, IMapper mapper)
        {
            _regionRepositories = regionRepositories;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await _regionRepositories.GetAllAsync();

            //Manual mapping of domain model to DTO

            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDto
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl
            //    });
            //}

            //Using Automapper to map data.
            var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);
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
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto newRegion)
        { 
            if(ModelState.IsValid)
            {
                var regionDomainModel = _mapper.Map<Region>(newRegion);
                regionDomainModel = await _regionRepositories.CreateAsync(regionDomainModel);
                //Map domain model back to DTO
                var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //Update a Region
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateRegionRequestDto updateRegionRequest)
        { 
            if(ModelState.IsValid)
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
            else
            {
                return BadRequest(ModelState);
            }
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
            var mapRegionDTO = _mapper.Map<RegionDto>(recordToDelete);
            return Ok(mapRegionDTO);
        }
    }
}
