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
        public async Task<ActionResult<(decimal avg, decimal sum)>> GetRecord(string name, uint? from, uint? to)
        {
            from ??= 0;
            to ??= uint.MaxValue;
            
            try
            {
                var result = await httpClient.GetStringAsync($"api/Calculation/{name}?from={from}&to={to}");

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                
                return StatusCode(500, ex.Message);
            }
            



          //if (_context.Records == null)
          //{
          //    return NotFound();
          //}
          //  var @record = await _context.Records.FindAsync(id);

          //  if (@record == null)
          //  {
          //      return NotFound();
          //  }
        }

        // POST: api/Records
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Record>> Post([FromBody]Record[] records)
        {
            _context.Records.AddRange(records);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecord", new { name = "multiple" }, records);
        }
    }
}
