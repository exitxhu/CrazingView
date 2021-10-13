using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrazingView.Db;
using CrazingView.Db.Entities;
using EFCore.BulkExtensions;

namespace CrazingView.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly CrazyContext _context;
        private bool _inprogress = false;
        private async Task recordGenerator(DbContext db, Strategy strat, IProgress<int> progress = null)
        {
            const int BulkSize = 100;
            var args = Enumerable.Repeat(0d, strat.InputCount).ToArray();
            var pat = Enumerable.Range(0, strat.InputCount).Aggregate("", (a, b) => a + $"{{{b}}}{(b == strat.InputCount - 1 ? null : ",")}");
            var toAdd = new List<Record>();
            for (int i = 0; i < strat.InputCount; i++)
                args[i] = strat.Inputs[i].MinValue;
            bool pending = true;
            while (pending)
            {
                for (int i = args.Length - 1; i >= 0; i--)
                {
                    if (args[i] < strat.Inputs[i].MaxValue)
                    {
                        args[i] += strat.Inputs[i].IncreaseStep;
                        toAdd.Add(new Record
                        {
                            StrategyId = strat.Id,
                            Value = string.Format(pat, args.Select(n=>n.ToString()).ToArray())
                        });
                        break;
                    }
                    if (i == 0)
                        pending = false;
                    args[i] = strat.Inputs[i].MinValue;
                }
                if (toAdd.Count == BulkSize)
                {
                    await db.BulkInsertAsync(toAdd);
                    toAdd.Clear();
                }
            }

            //if (progress != null)
            //    progress.Report();
        }
        public RecordsController(CrazyContext context)
        {
            _context = context;
        }
        [HttpPost("{strategyId}")]
        public async Task<IActionResult> GenerateStrategyRecords(long strategyId)
        {
            var strat = _context.Strategies.Include(n => n.Inputs).FirstOrDefault(n => n.Id == strategyId);
            if (strat == null)
                return NotFound();
           await recordGenerator(_context, strat).ConfigureAwait(false);
            return Ok();

        }
        // GET: api/Records
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Record>>> GetRecords()
        {
            return await _context.Records.ToListAsync();
        }

        // GET: api/Records/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Record>> GetRecord(long id)
        {
            var @record = await _context.Records.FindAsync(id);

            if (@record == null)
            {
                return NotFound();
            }

            return @record;
        }

        // PUT: api/Records/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecord(long id, Record @record)
        {
            if (id != @record.Id)
            {
                return BadRequest();
            }

            _context.Entry(@record).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecordExists(id))
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

        // POST: api/Records
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Record>> PostRecord(Record @record)
        {
            _context.Records.Add(@record);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecord", new { id = @record.Id }, @record);
        }

        // DELETE: api/Records/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecord(long id)
        {
            var @record = await _context.Records.FindAsync(id);
            if (@record == null)
            {
                return NotFound();
            }

            _context.Records.Remove(@record);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecordExists(long id)
        {
            return _context.Records.Any(e => e.Id == id);
        }
    }
}
