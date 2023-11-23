using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

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
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map Dto To Domain Model
            var walk = _mapper.Map<Walk>(addWalkRequestDto);
            var createdWalk = await _walkRepositories.CreateAsync(walk);

            if(createdWalk is null)
            {
                return NotFound();
            }
            //Map Domain Model to Dto
            var walkDto = _mapper.Map<WalkDto>(createdWalk);
            return Ok(walkDto);
        }
    }
}
