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
    public class ClazzController : ControllerBase
    {
        private readonly BackendContext _context;

        public ClazzController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/Clazzes
        [HttpGet]
        public IEnumerable<Clazz> GetClazz()
        {
            return _context.Clazz;
        }

        // GET: api/Clazzes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClazz([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var clazz = await _context.Clazz.FindAsync(id);

            if (clazz == null)
            {
                return NotFound();
            }
            return Ok(clazz);
        }

        // PUT: api/Clazzes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClazz([FromRoute] string id, [FromBody] Clazz clazz)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != clazz.Id)
            {
                return BadRequest();
            }
            
            _context.Entry(clazz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClazzExists(id))
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

        // POST: api/Clazzes
        [HttpPost]
        public async Task<IActionResult> PostClazz([FromBody] Clazz clazz)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _context.Clazz.Add(clazz);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClazz", new { id = clazz.Id }, clazz);
        }

        // DELETE: api/Clazzes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClazz([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var clazz = await _context.Clazz.FindAsync(id);
            if (clazz == null)
            {
                return NotFound();
            }
            
            _context.Clazz.Remove(clazz);
            await _context.SaveChangesAsync();

            return Ok(clazz);
        }

        private bool ClazzExists(string id)
        {
            return _context.Clazz.Any(e => e.Id == id);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string id)
        {
            var clazz = await _context.Clazz.FindAsync(id);
            if(clazz == null)
            {
                return NotFound("There're no class matched the search.");
            }
            return new JsonResult(clazz);
        }

        [HttpGet("{id}/Students")]
        public async Task<IActionResult> GetStudentsByClazz([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var listClazzStudent = _context.ClazzAccount.Where(cs => cs.ClazzId == id).ToArray();
            List<Account> listAccount = new List<Account>();
            foreach (var clazzStudent in listClazzStudent)
            {
                var account = _context.Account.Where(a => a.Id == clazzStudent.AccountId).FirstOrDefault();
                listAccount.Add(account);
            }

            if (listClazzStudent == null)
            {
                //return NotFound();
                return new JsonResult(listAccount);
            }
            return new JsonResult(listAccount);
        }

        [HttpGet("{id}/Subjects")]
        public async Task<IActionResult> GetSubjectsByClazz([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var listClazzSubjects = _context.ClazzSubject.Where(cs => cs.ClazzId == id).ToArray();
            List<Subject> listSubject = new List<Subject>();
            foreach (var clazzSubject in listClazzSubjects)
            {
                var subject = _context.Subject.Include(s=>s.Clazzes).Where(s => s.Id == clazzSubject.SubjectId).FirstOrDefault();
                listSubject.Add(subject);
            }

            if (listSubject == null)
            {
                return NotFound("There're no subjects in this class.");
            }
            return new JsonResult(listSubject);
        }
    }
}