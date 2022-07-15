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
        public async Task<ActionResult> Get(string name)
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

            return Ok(new
            {
                avg = await records.Select(x => x.V).AverageAsync(),
                sum = await records.Select(x => x.V).SumAsync(),

            });
        }
    }
}
