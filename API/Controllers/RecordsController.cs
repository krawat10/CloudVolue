using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API;
using API.Models;

namespace API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly HttpClient httpClient;

        public RecordsController(ApplicationContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            this.httpClient = httpClientFactory.CreateClient("Calculation");
        }

        // GET: api/5
        [HttpGet("{name}")]
        public async Task<ActionResult<Result>> GetRecord(string name, uint? from, uint? to)
        {
            if (name == null)
            {
                return BadRequest();
            }
            var records = _context.Records
                .Where(x => x.Name == name);

            if (!records.Any())
            {
                return NotFound();
            }

            var result = new Result
            {
                Avg = await records.Select(x => x.V).AverageAsync(),
                Sum = await records.Select(x => x.V).SumAsync(),
            };

            return Ok(result);

            // from ??= 0;
            // to ??= uint.MaxValue;
            //
            // if(string.IsNullOrWhiteSpace(name))
            //     return BadRequest();
            //
            // var result = await httpClient.GetAsync($"api/Calculation/{name}?from={from}&to={to}");
            // if(result.IsSuccessStatusCode)
            // {
            //     var json = await result.Content.ReadFromJsonAsync<Result>();
            //     return Ok(json);
            // }
            //     
            // if(result.StatusCode == System.Net.HttpStatusCode.NotFound)
            //     return NotFound();
            //
            // return StatusCode(500, "Service unavailable");
        }

        // POST: api/Records
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Record>> Post([FromBody]Record[] records)
        {
            _context.Records.AddRange(records);
            await _context.SaveChangesAsync();

            var names = string.Join(',', records.Select(x => x.Name).Distinct());

            return CreatedAtAction("GetRecord", new { name = names }, records);
        }

        public record Result 
        {
            public decimal Sum { get; set; }
            public decimal Avg { get; set; }
        }
    }
}
