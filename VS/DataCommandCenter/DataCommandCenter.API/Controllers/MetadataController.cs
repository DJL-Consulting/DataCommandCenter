﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataCommandCenter.DAL.DTO;
using DataCommandCenter.DAL.Services;
using DataCommandCenter.DAL.Models;
using Microsoft.AspNetCore.Hosting.Server;
using static DataCommandCenter.DAL.Services.IDCCRepository;
using Microsoft.AspNetCore.Cors;

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
        public async Task<ActionResult<IEnumerable<ServerDTO>>> GetServers()
        {
            var servers = await _repository.GetServers();

            return Ok(_mapper.Map<IEnumerable<ServerDTO>>(servers));   
        }

        [HttpPost]
        [Route("SearchObjects")]
        public async Task<ActionResult<IEnumerable<ObjectSearch>>> SearchObjects(SearchObjectTypes options)
        {
            var objects = await _repository.SearchObjects(options);

            return Ok(objects);
        }

        [HttpPost]
        [Route("GetMetadata")]
        public async Task<ActionResult<IEnumerable<ObjectSearch>>> GetMetadata(ObjectSearch selectedItem)
        {
            var objects = await _repository.GetMetadataForObject(selectedItem);

            return Ok(objects);
        }
    }
}
