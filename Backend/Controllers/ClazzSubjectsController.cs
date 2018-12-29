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
    public class ClazzSubjectsController : ControllerBase
    {
        private readonly BackendContext _context;

        public ClazzSubjectsController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/ClazzSubjects
        [HttpGet]
        public IEnumerable<ClazzSubject> GetClazzSubject()
        {
            return _context.ClazzSubject;
        }

        // GET: api/ClazzSubjects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClazzSubject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clazzSubject = await _context.ClazzSubject.FindAsync(id);

            if (clazzSubject == null)
            {
                return NotFound();
            }

            return Ok(clazzSubject);
        }

        // PUT: api/ClazzSubjects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClazzSubject([FromRoute] int id, [FromBody] ClazzSubject clazzSubject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clazzSubject.Id)
            {
                return BadRequest();
            }

            _context.Entry(clazzSubject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClazzSubjectExists(id))
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

        // POST: api/ClazzSubjects
        [HttpPost]
        public async Task<IActionResult> PostClazzSubject([FromBody] ClazzSubject clazzSubject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ClazzSubject.Add(clazzSubject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClazzSubject", new { id = clazzSubject.Id }, clazzSubject);
        }

        // DELETE: api/ClazzSubjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClazzSubject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clazzSubject = await _context.ClazzSubject.FindAsync(id);
            if (clazzSubject == null)
            {
                return NotFound();
            }

            _context.ClazzSubject.Remove(clazzSubject);
            await _context.SaveChangesAsync();

            return Ok(clazzSubject);
        }

        private bool ClazzSubjectExists(int id)
        {
            return _context.ClazzSubject.Any(e => e.Id == id);
        }
    }
}