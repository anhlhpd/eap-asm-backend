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
    public class CredentialsController : ControllerBase
    {
        private readonly BackendContext _context;

        public CredentialsController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/Credentials
        [HttpGet]
        public async Task<IActionResult> GetCredential()
        {
            if (HttpContext.Request.Query.ContainsKey("AccessToken"))
            {
                var cr = await _context.Credential.SingleOrDefaultAsync(c =>
                    c.AccessToken == HttpContext.Request.Query["AccessToken"].ToString());
                if (cr != null)
                {
                    return Ok(cr);
                }
            }
            return NotFound();
        }

        // GET: api/Credentials/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCredential([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var credential = await _context.Credential.FindAsync(id);

            if (credential == null)
            {
                return NotFound();
            }

            return Ok(credential);
        }

        // PUT: api/Credentials/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCredential([FromRoute] string id, [FromBody] Credential credential)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != credential.AccountId)
            {
                return BadRequest();
            }

            _context.Entry(credential).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CredentialExists(id))
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

        // POST: api/Credentials
        [HttpPost]
        public async Task<IActionResult> PostCredential([FromBody] Credential credential)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Credential.Add(credential);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCredential", new { id = credential.AccountId }, credential);
        }

        // DELETE: api/Credentials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCredential([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var credential = await _context.Credential.FindAsync(id);
            if (credential == null)
            {
                return NotFound();
            }

            _context.Credential.Remove(credential);
            await _context.SaveChangesAsync();

            return Ok(credential);
        }

        private bool CredentialExists(string id)
        {
            return _context.Credential.Any(e => e.AccountId == id);
        }
    }
}