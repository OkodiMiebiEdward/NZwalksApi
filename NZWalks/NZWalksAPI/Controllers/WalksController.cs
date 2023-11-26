using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepositories _walkRepositories;

        public WalksController(IMapper mapper, IWalkRepositories walkRepositories)
        {
            _mapper = mapper;
            _walkRepositories = walkRepositories;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
                //Map Dto To Domain Model
                var walk = _mapper.Map<Walk>(addWalkRequestDto);
                var createdWalk = await _walkRepositories.CreateAsync(walk);

                if (createdWalk is null)
                {
                    return NotFound();
                }
                //Map Domain Model to Dto
                var walkDto = _mapper.Map<WalkDto>(createdWalk);
                return Ok(walkDto);
        }

        [HttpGet]
        //https:localhost:7031/api/walks?filterOn=Name&filterQuery=map&sortBy=Name&isAscending=true
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery]string? filterQuery
            ,[FromQuery]string? sortBy, [FromQuery]bool? isAscending, 
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await _walkRepositories.GetAsync(filterOn,filterQuery,sortBy
                ,isAscending ?? true,pageNumber,pageSize);

            //Map walks to List<WalkDto>
            var walksDto = _mapper.Map<List<WalkDto>>(walksDomainModel);
            return Ok(walksDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var walkDomainModel =  await _walkRepositories.GetByIdAsync(id);
            if (walkDomainModel is null)
            {
                return NotFound();
            }

            //Map walk to walk Dto
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }

        //Controller for the Work update
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWorkRequestDto update)
        {
                var walkDomainModel = _mapper.Map<Walk>(update);
                var walk = await _walkRepositories.UpdateAsync(id, walkDomainModel);
                if (walk is null)
                {
                  return NotFound();
                }
                return Ok(_mapper.Map<WalkDto>(walk));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomainModel = await  _walkRepositories.DeleteAsync(id);
            if(walkDomainModel is null)
            {
              return NotFound();
            }
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }
    }
}
