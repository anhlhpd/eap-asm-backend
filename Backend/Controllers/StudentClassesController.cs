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
    public class StudentClassesController : ControllerBase
    {
        private readonly BackendContext _context;

        public StudentClassesController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/StudentClasses
        [HttpGet]
        public IEnumerable<StudentClass> GetStudentClass()
        {
            return _context.StudentClass;
        }

        // GET: api/StudentClasses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentClass([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentClass = await _context.StudentClass.FindAsync(id);

            if (studentClass == null)
            {
                return NotFound();
            }

            return Ok(studentClass);
        }

        // PUT: api/StudentClasses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentClass([FromRoute] string id, [FromBody] StudentClass studentClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studentClass.StudentClassId)
            {
                return BadRequest();
            }

            _context.Entry(studentClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentClassExists(id))
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

        // POST: api/StudentClasses
        [HttpPost]
        public async Task<IActionResult> PostStudentClass([FromBody] StudentClass studentClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StudentClass.Add(studentClass);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentClass", new { id = studentClass.StudentClassId }, studentClass);
        }

        // DELETE: api/StudentClasses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentClass([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentClass = await _context.StudentClass.FindAsync(id);
            if (studentClass == null)
            {
                return NotFound();
            }

            _context.StudentClass.Remove(studentClass);
            await _context.SaveChangesAsync();

            return Ok(studentClass);
        }

        private bool StudentClassExists(string id)
        {
            return _context.StudentClass.Any(e => e.StudentClassId == id);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string id)
        {
            var studentClass = await _context.StudentClass.FindAsync(id);
            return new JsonResult(studentClass);
        }
    }
}