using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrazingView.Db;
using CrazingView.Db.Entities;

namespace CrazingView.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StrategiesController : ControllerBase
    {
        private readonly CrazyContext _context;

        public StrategiesController(CrazyContext context)
        {
            _context = context;
        }

        // GET: api/Strategies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Strategy>>> GetStrategies()
        {
            return await _context.Strategies.AsNoTracking().Include(n=>n.Inputs).ToListAsync();
        }

        // GET: api/Strategies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Strategy>> GetStrategy(long id)
        {
            var strategy = await _context.Strategies.AsNoTracking().Include(n => n.Inputs).FirstOrDefaultAsync(n=>n.Id == id);

            if (strategy == null)
            {
                return NotFound();
            }

            return strategy;
        }

        // PUT: api/Strategies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStrategy(long id, Strategy strategy)
        {
            if (id != strategy.Id)
            {
                return BadRequest();
            }

            _context.Entry(strategy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StrategyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Strategies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Strategy>> PostStrategy(Strategy strategy)
        {
            _context.Strategies.Add(strategy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStrategy", new { id = strategy.Id }, strategy);
        }

        // DELETE: api/Strategies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStrategy(long id)
        {
            var strategy = await _context.Strategies.FindAsync(id);
            if (strategy == null)
            {
                return NotFound();
            }

            _context.Strategies.Remove(strategy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StrategyExists(long id)
        {
            return _context.Strategies.Any(e => e.Id == id);
        }
    }
}
