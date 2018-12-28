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
    public class AuthenticationController : ControllerBase
    {
        private readonly BackendContext _context;

        public AuthenticationController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/Authentication
        [HttpGet]
        public async Task<IActionResult> GetCredential()
        {
           
            if (HttpContext.Request.Query.ContainsKey("AccessToken"))
            {
                var cr = await _context.Credential.SingleOrDefaultAsync(c =>
                    c.AccessToken == HttpContext.Request.Query["AccessToken"].ToString());
                if (cr != null)
                {
                    var currentRole = _context.Role.Find(_context.AccountRoles.SingleOrDefault(ar=>ar.AccountId == cr.OwnerId).RoleId);
                    return Ok(currentRole);
                }
            }
            return NotFound();
        }
        // GET: api/Authentication/5
        [HttpGet("{CheckAdmin}")]
        public async Task<IActionResult> CheckAdmin([FromRoute] string CheckAdmin)
        {
           if (HttpContext.Request.Query.ContainsKey("AccessToken"))
            {
                var cr = await _context.Credential.SingleOrDefaultAsync(c =>
                    c.AccessToken == HttpContext.Request.Query["AccessToken"].ToString());
                if (cr != null)
                {
                    if (_context.AccountRoles.SingleOrDefault(ar => ar.AccountId == cr.OwnerId) != null)
                    {
                        if (_context.Role.SingleOrDefault(r => r.RoleId == _context.AccountRoles.SingleOrDefault(ar => ar.AccountId == cr.OwnerId).RoleId).Name == "Admin")
                        {
                            return Ok();
                        }
                    }
                }
            }
            return NotFound();
        }

        // PUT: api/Authentication/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCredential([FromRoute] string id, [FromBody] Credential credential)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != credential.OwnerId)
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

        // POST: api/Authentication
        [HttpPost]
        public async Task<IActionResult> PostCredential([FromBody] Credential credential)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Credential.Add(credential);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCredential", new { id = credential.OwnerId }, credential);
        }

        // DELETE: api/Authentication/5
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
            return _context.Credential.Any(e => e.OwnerId == id);
        }
    }
}