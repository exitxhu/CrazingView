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
    public class SessionsController : ControllerBase
    {
        private readonly CrazyContext _context;

        public SessionsController(CrazyContext context)
        {
            _context = context;
        }

        // GET: api/Sessions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Session>>> GetSessions()
        {
            return await _context.Sessions.ToListAsync();
        }


        [HttpPost("[action]")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> StartNewSession(long stratId, string tabId)
        {
            var strat = await _context.Strategies.FirstOrDefaultAsync(n => n.Id == stratId);
            var lastSession = await _context.Sessions.OrderByDescending(n => n.StartTime).FirstOrDefaultAsync(n => n.StrategyId == stratId);
            var records = _context.Records
                .OrderBy(n => n.Id)
                .Where(n => n.StrategyId == stratId && (lastSession == null ? true : n.Id > lastSession.ChunkEndId))
                .Take(100)
                .ToList();
            var session = new Session
            {
                ChunkCount = records.Count,
                ChunkEndId = records.LastOrDefault().Id,
                ChunkStartId = records.FirstOrDefault().Id,
                PairName = strat.PairName,
                Status = SessionStatus.Pending,
                TabId = tabId,
                StartTime = DateTime.Now,
                StrategyId = stratId
            };
            var log = new SessionLog
            {
                LogTime = DateTime.Now,
                Session = session,
                LogType = LogType.Started,

            };
            _context.Add(session);
            _context.Add(log);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                sid = session.Id,
                records = records.Select(n => new { n.Id, n.Value })
            });
        }

        // GET: api/Sessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Session>> GetSession(long id)
        {
            var session = await _context.Sessions.FindAsync(id);

            if (session == null)
            {
                return NotFound();
            }

            return session;
        }

        // PUT: api/Sessions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSession(long id, Session session)
        {
            if (id != session.Id)
            {
                return BadRequest();
            }

            _context.Entry(session).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(id))
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

        // POST: api/Sessions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Session>> PostSession(Session session)
        {
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSession", new { id = session.Id }, session);
        }

        // DELETE: api/Sessions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(long id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SessionExists(long id)
        {
            return _context.Sessions.Any(e => e.Id == id);
        }
    }
}
