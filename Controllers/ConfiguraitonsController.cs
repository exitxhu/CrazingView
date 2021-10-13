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
    public class ConfiguraitonsController : ControllerBase
    {
        private readonly CrazyContext _context;

        public ConfiguraitonsController(CrazyContext context)
        {
            _context = context;
        }

        // GET: api/Configuraitons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Configuraiton>>> GetConfiguraitons()
        {
            return await _context.Configuraitons.ToListAsync();
        }

        // GET: api/Configuraitons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Configuraiton>> GetConfiguraiton(int id)
        {
            var configuraiton = await _context.Configuraitons.FindAsync(id);

            if (configuraiton == null)
            {
                return NotFound();
            }

            return configuraiton;
        }

        // PUT: api/Configuraitons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConfiguraiton(int id, Configuraiton configuraiton)
        {
            if (id != configuraiton.Id)
            {
                return BadRequest();
            }

            _context.Entry(configuraiton).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfiguraitonExists(id))
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

        // POST: api/Configuraitons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Configuraiton>> PostConfiguraiton(Configuraiton configuraiton)
        {
            _context.Configuraitons.Add(configuraiton);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConfiguraiton", new { id = configuraiton.Id }, configuraiton);
        }

        // DELETE: api/Configuraitons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfiguraiton(int id)
        {
            var configuraiton = await _context.Configuraitons.FindAsync(id);
            if (configuraiton == null)
            {
                return NotFound();
            }

            _context.Configuraitons.Remove(configuraiton);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConfiguraitonExists(int id)
        {
            return _context.Configuraitons.Any(e => e.Id == id);
        }
    }
}
