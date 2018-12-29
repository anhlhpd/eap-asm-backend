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
    public class GeneralInformationsController : ControllerBase
    {
        private readonly BackendContext _context;

        public GeneralInformationsController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/GeneralInformations
        [HttpGet]
        public IEnumerable<GeneralInformation> GetGeneralInformation()
        {
            return _context.GeneralInformation;
        }

        // GET: api/GeneralInformations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGeneralInformation([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var generalInformation = await _context.GeneralInformation.FindAsync(id);

            if (generalInformation == null)
            {
                return NotFound();
            }

            return Ok(generalInformation);
        }

        // PUT: api/GeneralInformations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeneralInformation([FromRoute] string id, [FromBody] GeneralInformation generalInformation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != generalInformation.AccountId)
            {
                return BadRequest();
            }

            _context.Entry(generalInformation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneralInformationExists(id))
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

        // POST: api/GeneralInformations
        [HttpPost]
        public async Task<IActionResult> PostGeneralInformation([FromBody] GeneralInformation generalInformation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GeneralInformation.Add(generalInformation);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GeneralInformationExists(generalInformation.AccountId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetGeneralInformation", new { id = generalInformation.AccountId }, generalInformation);
        }

        // DELETE: api/GeneralInformations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeneralInformation([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var generalInformation = await _context.GeneralInformation.FindAsync(id);
            if (generalInformation == null)
            {
                return NotFound();
            }

            _context.GeneralInformation.Remove(generalInformation);
            await _context.SaveChangesAsync();

            return Ok(generalInformation);
        }

        private bool GeneralInformationExists(string id)
        {
            return _context.GeneralInformation.Any(e => e.AccountId == id);
        }

        [HttpPost("Student")]
        public async Task<IActionResult> PostGeneralInformationInformation([FromBody] GeneralInformation generalInformation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GeneralInformation.Add(generalInformation);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GeneralInformationExists(generalInformation.AccountId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            var name = generalInformation.FirstName;
            var accountUsername = generalInformation.FirstName + generalInformation.LastName[0] + generalInformation.AccountId;
            var salt = SecurityHandle.PasswordHandle.GetInstance().GenerateSalt();
            Account account = new Account()
            {
                Id = generalInformation.AccountId,
                Username = accountUsername.ToLower(),
                Email = accountUsername.ToLower() + "@gmail.com",
                Salt = salt,
                Password = SecurityHandle.PasswordHandle.GetInstance().EncryptPassword(accountUsername.ToLower(), salt)                
            };
            _context.Account.Add(account);
            _context.SaveChanges();

            return CreatedAtAction("GetGeneralInformation", new { id = generalInformation.AccountId }, generalInformation);
        }
    }
}