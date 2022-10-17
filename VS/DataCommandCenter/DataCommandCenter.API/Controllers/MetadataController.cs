﻿using AutoMapper;
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
        public async Task<ActionResult<MetadataDTO>> GetServers()
        {
            try
            {
                var retObj = new MetadataDTO();

                var servers = await _repository.GetAllServers();
                var dbs = new List<SqlDatabase>();
                var objs = new List<SqlObject>();
                var cols = new List<SqlColumn>();
                var ints = new List<Integration>();

                retObj.Servers = _mapper.Map<IEnumerable<ServerDTO>>(servers);
                retObj.Databases = _mapper.Map<IEnumerable<DatabaseDTO>>(dbs);
                retObj.Objects = _mapper.Map<IEnumerable<ObjectDTO>>(objs);
                retObj.Columns = _mapper.Map<IEnumerable<ColumnDTO>>(cols);
                retObj.Integrations = _mapper.Map<IEnumerable<IntegrationDTO>>(ints);

                return Ok(retObj);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [Route("GetDatabases")]
        public async Task<ActionResult<MetadataDTO>> GetDatabases()
        {
            try
            {
                var retObj = new MetadataDTO();

                var servers = new List<Server>(); //await _repository.GetAllServers();
                var dbs = await _repository.GetAllDBs(); 
                var objs = new List<SqlObject>();
                var cols = new List<SqlColumn>();
                var ints = new List<Integration>();

                retObj.Servers = _mapper.Map<IEnumerable<ServerDTO>>(servers);
                retObj.Databases = _mapper.Map<IEnumerable<DatabaseDTO>>(dbs);
                retObj.Objects = _mapper.Map<IEnumerable<ObjectDTO>>(objs);
                retObj.Columns = _mapper.Map<IEnumerable<ColumnDTO>>(cols);
                retObj.Integrations = _mapper.Map<IEnumerable<IntegrationDTO>>(ints);

                return Ok(retObj);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [Route("GetIntegrations")]
        public async Task<ActionResult<MetadataDTO>> GetIntegrations()
        {
            try
            {
                var retObj = new MetadataDTO();

                var servers = new List<Server>(); 
                var dbs = new List<SqlDatabase>();
                var objs = new List<SqlObject>();
                var cols = new List<SqlColumn>();
                var ints = await _repository.GetAllIntegrations();

                retObj.Servers = _mapper.Map<IEnumerable<ServerDTO>>(servers);
                retObj.Databases = _mapper.Map<IEnumerable<DatabaseDTO>>(dbs);
                retObj.Objects = _mapper.Map<IEnumerable<ObjectDTO>>(objs);
                retObj.Columns = _mapper.Map<IEnumerable<ColumnDTO>>(cols);
                retObj.Integrations = _mapper.Map<IEnumerable<IntegrationDTO>>(ints);

                return Ok(retObj);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
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
                var ints = new List<Integration>();

                retObj.Servers = _mapper.Map<IEnumerable<ServerDTO>>(servers);
                retObj.Databases = _mapper.Map<IEnumerable<DatabaseDTO>>(dbs);
                retObj.Objects = _mapper.Map<IEnumerable<ObjectDTO>>(objs);
                retObj.Columns = _mapper.Map<IEnumerable<ColumnDTO>>(cols);
                retObj.Integrations = _mapper.Map<IEnumerable<IntegrationDTO>>(ints);

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
