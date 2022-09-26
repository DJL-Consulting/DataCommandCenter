using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataCommandCenter.DAL.DTO;
using DataCommandCenter.DAL.Services;
using DataCommandCenter.DAL.Models;
using Microsoft.AspNetCore.Hosting.Server;

namespace DataCommandCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetadataController : ControllerBase
    {
        private readonly IDCCRepository _repository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public MetadataController(IDCCRepository DCCRepository,
            IMapper mapper)
        {
            _repository = DCCRepository ??
                throw new ArgumentNullException(nameof(DCCRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Route("GetServers")]
        public async Task<ActionResult<IEnumerable<ObjectSearch>>> GetServers()
        {
            var servers = await _repository.GetServers();

            return Ok(_mapper.Map<IEnumerable<ServerDTO>>(servers));   
        }

        [HttpGet]
        [Route("SearchObjects")]
        public async Task<ActionResult<IEnumerable<ObjectSearch>>> SearchObjects(String queryString)
        {
            var objects = await _repository.SearchObjects(queryString);

            return Ok(objects);
        }

    }
}
