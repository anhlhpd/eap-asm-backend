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
    public class SubjectsController : ControllerBase
    {
        private readonly BackendContext _context;

        public SubjectsController(BackendContext context)
        {
            _context = context;
        }
        // Student: get all subjects of 1 student, including start dates
        // GET: api/Subjects/Student/GetAllSubject
        [HttpGet("Student/GetAllSubject")]
        public IEnumerable<ClazzSubject> StudentGetAllSubject()
        {
            string tokenHeader = Request.Headers["Authorization"];
            var token = tokenHeader.Replace("Basic ", "");
            var cr = _context.Credential.SingleOrDefault(c =>
                   c.AccessToken == token);
            IEnumerable<ClazzAccount> clazzAccounts = _context.ClazzAccount.Where(ca => ca.AccountId == cr.OwnerId);
            IEnumerable<ClazzSubject> clazzSubjects = null;
            foreach(var clazzAccount in clazzAccounts)
            {
                var singleCSs = _context.ClazzSubject.Where(cs => cs.ClazzId == clazzAccount.ClazzId);
                foreach (var singleCS in singleCSs)
                {
                    clazzSubjects.Append(singleCS);
                }
            }
            return clazzSubjects;
        }

        // Manager: get all subjects of 1 student, including start dates
        // GET: api/Subjects/Manager/GetAllSubjectOneStudent
        [HttpGet("Manager/GetAllSubjectOneStudent")]
        public IEnumerable<ClazzSubject> ManagerGetAllSubjectOneStudent(string studentId)
        {
            IEnumerable<ClazzAccount> clazzAccounts = _context.ClazzAccount.Where(ca => ca.AccountId == studentId);
            IEnumerable<ClazzSubject> clazzSubjects = null;
            foreach (var clazzAccount in clazzAccounts)
            {
                var singleCSs = _context.ClazzSubject.Where(cs => cs.ClazzId == clazzAccount.ClazzId);
                foreach (var singleCS in singleCSs)
                {
                    clazzSubjects.Append(singleCS);
                }
            }
            return clazzSubjects;
        }


        // GET: api/Subjects
        [HttpGet]
        public async Task<IActionResult> GetSubject()
        {

            return Ok();
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubject([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subject = await _context.Subject.FindAsync(id);

            if (subject == null)
            {
                return NotFound();
            }

            return Ok(subject);
        }

        // PUT: api/Subjects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubject([FromRoute] string id, [FromBody] Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subject.Id)
            {
                return BadRequest();
            }

            _context.Entry(subject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(id))
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

        // POST: api/Subjects
        [HttpPost]
        public async Task<IActionResult> PostSubject([FromBody] Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Subject.Add(subject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubject", new { id = subject.Id }, subject);
        }

        // DELETE: api/Subjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subject = await _context.Subject.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            _context.Subject.Remove(subject);
            await _context.SaveChangesAsync();

            return Ok(subject);
        }

        private bool SubjectExists(string id)
        {
            return _context.Subject.Any(e => e.Id == id);
        }
    }
}