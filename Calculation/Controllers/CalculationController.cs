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
    [Route("api/Calculation")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public CalculationController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Records/5
        [HttpGet("{name}")]
        public async Task<ActionResult> Get(string name, uint from, uint to)
        {
          if (name == null)
          {
              return BadRequest();
          }

          var records = _context.Records
              .Where(x => x.Name == name);
                            // x.T > from && 
                            // x.T < to);

            if (!records.Any())
            {
                return NotFound();
            }

            var result = new Response {
                Avg = await records.Select(x => x.V).AverageAsync(),
                Sum = await records.Select(x => x.V).SumAsync(),
            };

            return Ok(result);
        }
    }

    record Response
    {
        public decimal Avg { get; set; }
        public decimal Sum { get; set; }
    }
}
