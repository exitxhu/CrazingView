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
    public class SessionLogsController : ControllerBase
    {
        private readonly CrazyContext _context;

        public SessionLogsController(CrazyContext context)
        {
            _context = context;
        }

        // GET: api/SessionLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionLog>>> GetSessionLogs()
        {
            return await _context.SessionLogs.ToListAsync();
        }

        // GET: api/SessionLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SessionLog>> GetSessionLog(long id)
        {
            var sessionLog = await _context.SessionLogs.FindAsync(id);

            if (sessionLog == null)
            {
                return NotFound();
            }

            return sessionLog;
        }

        // PUT: api/SessionLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSessionLog(long id, SessionLog sessionLog)
        {
            if (id != sessionLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(sessionLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionLogExists(id))
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

        // POST: api/SessionLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SessionLog>> PostSessionLog(SessionLog sessionLog)
        {
            _context.SessionLogs.Add(sessionLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSessionLog", new { id = sessionLog.Id }, sessionLog);
        }

        // DELETE: api/SessionLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSessionLog(long id)
        {
            var sessionLog = await _context.SessionLogs.FindAsync(id);
            if (sessionLog == null)
            {
                return NotFound();
            }

            _context.SessionLogs.Remove(sessionLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SessionLogExists(long id)
        {
            return _context.SessionLogs.Any(e => e.Id == id);
        }
    }
}
