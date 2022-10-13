using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataCommandCenter.DAL.DTO;
using DataCommandCenter.DAL.Services;
using DataCommandCenter.DAL.Models;
using Microsoft.AspNetCore.Hosting.Server;
using static DataCommandCenter.DAL.Services.IDCCRepository;
using Microsoft.AspNetCore.Cors;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace DataCommandCenter.API.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<ServerDTO>>> GetServers()
        {
            var servers = await _repository.GetServers();

            return Ok(_mapper.Map<IEnumerable<ServerDTO>>(servers));   
        }

        [HttpPost]
        [Route("SearchObjects")]
        public async Task<ActionResult<IEnumerable<ObjectSearch>>> SearchObjects(SearchObjectTypes options)
        {
            try
            {
                var objects = await _repository.SearchObjects(options);

                return Ok(objects);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }


        [HttpPost]
        [Route("GetMetadataForObject")]
        public async Task<ActionResult<MetadataDTO>> GetMetadataForObject(ObjectSearch selectedItem)
        {
            try
            {
                var retObj = new MetadataDTO();

                var servers = await _repository.GetServersForObject(selectedItem);
                var dbs = await _repository.GetDBsForObject(selectedItem);
                var objs = await _repository.GetObjectsForObject(selectedItem);
                var cols = await _repository.GetColumnsForObject(selectedItem);

                retObj.Servers = _mapper.Map<IEnumerable<ServerDTO>>(servers);
                retObj.Databases = _mapper.Map<IEnumerable<DatabaseDTO>>(dbs);
                retObj.Objects = _mapper.Map<IEnumerable<ObjectDTO>>(objs);
                retObj.Columns = _mapper.Map<IEnumerable<ColumnDTO>>(cols);

                return Ok(retObj);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("GetLineageForObject")]
        public async Task<ActionResult<LineageDTO>> GetLineageForObject(ObjectSearch selectedItem)
        {
            try
            {
                var retObj = new LineageDTO();

                var (flows, nodes) = await _repository.GetLineageForObject(selectedItem);
                retObj.Nodes = _mapper.Map<IEnumerable<LineageNode>>(nodes);
                retObj.Flows = _mapper.Map<IEnumerable<LineageLink>>(flows);

                foreach (var flow in retObj.Flows)
                {
                    flow.Integration = _mapper.Map<IntegrationDTO>(flow.Integration);
                }

                return Ok(retObj);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
