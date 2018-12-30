using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarksController : ControllerBase
    {
        private readonly BackendContext _context;

        public MarksController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/Marks
        [HttpGet]
        public IEnumerable<Mark> GetMark()
        {
            return _context.Mark;
        }

        // GET: api/Marks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMark([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mark = await _context.Mark.FindAsync(id);

            if (mark == null)
            {
                return NotFound();
            }

            return Ok(mark);
        }

        // PUT: api/Marks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMark([FromRoute] long id, [FromBody] Mark mark)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mark.Id)
            {
                return BadRequest();
            }

            _context.Entry(mark).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarkExists(id))
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

        // POST: api/Marks
        [HttpPost]
        public async Task<IActionResult> PostMark([FromBody] Mark mark)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Mark.Add(mark);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMark", new { id = mark.Id }, mark);
        }

        // DELETE: api/Marks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMark([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mark = await _context.Mark.FindAsync(id);
            if (mark == null)
            {
                return NotFound();
            }

            _context.Mark.Remove(mark);
            await _context.SaveChangesAsync();

            return Ok(mark);
        }

        private bool MarkExists(long id)
        {
            return _context.Mark.Any(e => e.Id == id);
        }
    }
}