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
    public class StrategyInputsController : ControllerBase
    {
        private readonly CrazyContext _context;

        public StrategyInputsController(CrazyContext context)
        {
            _context = context;
        }

        // GET: api/StrategyInputs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StrategyInput>>> GetStrategyInputs()
        {
            return await _context.StrategyInputs.ToListAsync();
        }

        // GET: api/StrategyInputs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StrategyInput>> GetStrategyInput(int id)
        {
            var strategyInput = await _context.StrategyInputs.FindAsync(id);

            if (strategyInput == null)
            {
                return NotFound();
            }

            return strategyInput;
        }

        // PUT: api/StrategyInputs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStrategyInput(int id, StrategyInput strategyInput)
        {
            if (id != strategyInput.Id)
            {
                return BadRequest();
            }

            _context.Entry(strategyInput).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StrategyInputExists(id))
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

        // POST: api/StrategyInputs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StrategyInput>> PostStrategyInput(StrategyInput strategyInput)
        {
            _context.StrategyInputs.Add(strategyInput);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStrategyInput", new { id = strategyInput.Id }, strategyInput);
        }

        // DELETE: api/StrategyInputs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStrategyInput(int id)
        {
            var strategyInput = await _context.StrategyInputs.FindAsync(id);
            if (strategyInput == null)
            {
                return NotFound();
            }

            _context.StrategyInputs.Remove(strategyInput);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StrategyInputExists(int id)
        {
            return _context.StrategyInputs.Any(e => e.Id == id);
        }
    }
}
