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
    public class StudentClassAccountsController : ControllerBase
    {
        private readonly BackendContext _context;

        public StudentClassAccountsController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/StudentClassAccounts
        [HttpGet]
        public IEnumerable<StudentClassAccount> GetStudentClassAccount()
        {
            return _context.StudentClassAccount;
        }

        // GET: api/StudentClassAccounts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentClassAccount([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentClassAccount = await _context.StudentClassAccount.FindAsync(id);

            if (studentClassAccount == null)
            {
                return NotFound();
            }

            return Ok(studentClassAccount);
        }

        // PUT: api/StudentClassAccounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentClassAccount([FromRoute] string id, [FromBody] StudentClassAccount studentClassAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studentClassAccount.StudentClassId)
            {
                return BadRequest();
            }

            _context.Entry(studentClassAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentClassAccountExists(id))
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

        // POST: api/StudentClassAccounts
        [HttpPost]
        public async Task<IActionResult> PostStudentClassAccount([FromBody] StudentClassAccount studentClassAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StudentClassAccount.Add(studentClassAccount);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentClassAccountExists(studentClassAccount.StudentClassId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudentClassAccount", new { id = studentClassAccount.StudentClassId }, studentClassAccount);
        }

        // DELETE: api/StudentClassAccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentClassAccount([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentClassAccount = await _context.StudentClassAccount.FindAsync(id);
            if (studentClassAccount == null)
            {
                return NotFound();
            }

            _context.StudentClassAccount.Remove(studentClassAccount);
            await _context.SaveChangesAsync();

            return Ok(studentClassAccount);
        }

        private bool StudentClassAccountExists(string id)
        {
            return _context.StudentClassAccount.Any(e => e.StudentClassId == id);
        }
    }
}