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
    public class PersonalInformationsController : ControllerBase
    {
        private readonly BackendContext _context;

        public PersonalInformationsController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/PersonalInformations
        [HttpGet]
        public IEnumerable<PersonalInformation> GetPersonalInformation()
        {
            return _context.PersonalInformation;
        }

        // GET: api/PersonalInformations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonalInformation([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personalInformation = await _context.PersonalInformation.FindAsync(id);

            if (personalInformation == null)
            {
                return NotFound();
            }

            return Ok(personalInformation);
        }

        // PUT: api/PersonalInformations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonalInformation([FromRoute] string id, [FromBody] PersonalInformation personalInformation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != personalInformation.AccountId)
            {
                return BadRequest();
            }

            _context.Entry(personalInformation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonalInformationExists(id))
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

        // POST: api/PersonalInformations
        [HttpPost]
        public async Task<IActionResult> PostPersonalInformation([FromBody] PersonalInformation personalInformation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PersonalInformation.Add(personalInformation);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonalInformationExists(personalInformation.AccountId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetPersonalInformation", new { id = personalInformation.AccountId }, personalInformation);
        }

        // DELETE: api/PersonalInformations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonalInformation([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personalInformation = await _context.PersonalInformation.FindAsync(id);
            if (personalInformation == null)
            {
                return NotFound();
            }

            _context.PersonalInformation.Remove(personalInformation);
            await _context.SaveChangesAsync();

            return Ok(personalInformation);
        }

        private bool PersonalInformationExists(string id)
        {
            return _context.PersonalInformation.Any(e => e.AccountId == id);
        }

        [HttpPost("Student")]
        public async Task<IActionResult> PostStudentPersonalInformation([FromBody] PersonalInformation personalInformation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PersonalInformation.Add(personalInformation);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonalInformationExists(personalInformation.AccountId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            var name = personalInformation.FirstName;
            var accountUsername = personalInformation.FirstName + personalInformation.LastName[0];
            Account account = new Account()
            {
                AccountId = new { id = personalInformation.AccountId},
                Username = accountUsername,
                Email = accountUsername + "@gmail.com",
                Password = accountUsername
            };
            _context.Account.Add(account);
            _context.SaveChanges();

            return CreatedAtAction("GetPersonalInformation", new { id = personalInformation.AccountId }, personalInformation);
        }
    }
}