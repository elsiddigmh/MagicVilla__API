﻿using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers.v2
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaNumberAPIController : Controller
    {
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IVillaRepository _dbVilla;
        protected readonly IMapper _mapper;
        private readonly APIResponse _response;

        public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, IVillaRepository dbVilla, IMapper mapper)
        {
            _dbVillaNumber = dbVillaNumber;
            _dbVilla = dbVilla;
            _mapper = mapper;
            _response = new();
        }

        //[MapToApiVersion("2.0")]
        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Elsiddig", "Mohamed" };
        }






    }
}
