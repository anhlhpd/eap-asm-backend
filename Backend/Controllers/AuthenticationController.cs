using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using SecurityHandle;

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
        // POST: api/Login
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] string username, string password, string clientId)
        {
            // find 1 account with matching username in Account
            var ac = await _context.Account.SingleOrDefaultAsync(a =>
                    a.Username == username);
            // verify clientId to be either STU or TCH first
            var isCorrectClient = ac.AccountId.StartsWith(clientId);
            if (ac != null && isCorrectClient == true)
            {
                // check if account is logged in elsewhere
                var cr = await _context.Credential.SingleOrDefaultAsync(c =>
                    c.AccountId == ac.AccountId);
                if(cr == null) // if account has never logged in
                {
                    // check matching password
                    if (PasswordHandle.GetInstance().EncryptPassword(ac.Password, ac.Salt) == PasswordHandle.GetInstance().EncryptPassword(password, ac.Salt))
                    {
                        // create new credential with AccountId
                        var firstCredential = new Credential {
                            AccountId = ac.AccountId,
                            AccessToken = TokenHandle.GetInstance().GenerateToken()
                        };

                        _context.Credential.Add(firstCredential);
                        await _context.SaveChangesAsync();
                        // save token
                        return Ok(TokenHandle.GetInstance().GenerateToken());
                    }
                }
                else if(cr.AccessToken == null) // if 1 credential exists and accessToken was deleted
                {
                    // check matching password
                    if (PasswordHandle.GetInstance().EncryptPassword(ac.Password, ac.Salt) == PasswordHandle.GetInstance().EncryptPassword(password, ac.Salt))
                    {
                        // save token
                        var accessToken = TokenHandle.GetInstance().GenerateToken();
                        cr.AccessToken = accessToken;
                        return Ok(accessToken);
                    }
                }
            }
            return NotFound();
        }

        // POST: api/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.Request.Query.ContainsKey("AccessToken"))
            {
                var cr = await _context.Credential.SingleOrDefaultAsync(c =>
                    c.AccessToken == HttpContext.Request.Query["AccessToken"].ToString());
                if (cr != null)
                {
                    // just delete accessToken from credential
                    try
                    {
                        cr.AccessToken = null;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    return Ok();
                }
            }
            return BadRequest("You're already logged out!");
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
                    return Ok(cr);
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
                    if (_context.AccountRoles.SingleOrDefault(ar => ar.AccountId == cr.AccountId) != null)
                    {
                        if (_context.Role.SingleOrDefault(r => r.RoleId == _context.AccountRoles.SingleOrDefault(ar => ar.AccountId == cr.AccountId).RoleId).Name == "Admin")
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

            return CreatedAtAction("GetCredential", new { id = credential.AccountId }, credential);
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
            return _context.Credential.Any(e => e.AccountId == id);
        }
    }
}