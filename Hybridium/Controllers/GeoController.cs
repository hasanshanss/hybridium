using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper;
using Hybridium.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using static Hybridium.Models.CsvMappers;

namespace Hybridium.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoController : ControllerBase
    {
        private IConfiguration conf { get; }
        private hybridContext _dbContext;

        public GeoController(IConfiguration config,
                            hybridContext dbContext)
        {
            conf = config;
            _dbContext = dbContext;
        }

        // GET: api/Geo?ip=12.19.50.0/23
        [HttpGet]
        public string Get(string ip)
        {
            long? geonameId = _dbContext.GeoipBlocks.Where(m => m.Network.Equals(ip)).Select(m=>m.GeonameId).FirstOrDefault();
            return JsonConvert.SerializeObject(_dbContext.GeoipLocations.Where(m => m.GeonameId.Equals(geonameId)).ToList());
        }

    }
}
