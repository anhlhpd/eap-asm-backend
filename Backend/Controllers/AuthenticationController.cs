using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using SecurityHandle;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using System.Globalization;
using System.Net;


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

        // POST: api/Authentication/StudentLogin
        [Route("StudentLogin")]
        [HttpPost]
        public async Task<IActionResult> StudentLogin(LoginInformation loginInformation)
        {
            
            // find 1 account with matching username in Account
            var ac = await _context.Account.SingleOrDefaultAsync(a =>
                    a.Username == loginInformation.Username);
            
            if (ac != null)
            {
                var isCorrectClient = ac.Id.StartsWith("STU");
                if (isCorrectClient)
                {
                    // check matching password
                    if (ac.Password == PasswordHandle.GetInstance().EncryptPassword(loginInformation.Password, ac.Salt))
                    {
                        // check if account is logged in elsewhere
                        var cr = await _context.Credential.SingleOrDefaultAsync(c =>
                            c.OwnerId == ac.Id);
                        var accessToken = TokenHandle.GetInstance().GenerateToken();
                        if (cr != null) // if account has logged in
                        {
                            cr.AccessToken = accessToken;
                            // save token
                            _context.Credential.Update(cr);
                            await _context.SaveChangesAsync();
                            return Ok(accessToken);
                        }
                        // create new credential with AccountId
                        var firstCredential = new Credential
                        {
                            OwnerId = ac.Id,
                            AccessToken = accessToken
                        };
                        _context.Credential.Add(firstCredential);
                        await _context.SaveChangesAsync();
                        // save token
                        return Ok(accessToken);
                    }

                    
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return new JsonResult(new ResponseError("UserName or Password is incorrect", (int)HttpStatusCode.Forbidden));
                }
                
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return new JsonResult(new ResponseError("Client is Wrong", (int)HttpStatusCode.Forbidden));
                
            }
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return new JsonResult(new ResponseError("UserName or Password is incorrect", (int)HttpStatusCode.Forbidden));
        }

        [Route("StaffLogin")]
        [HttpPost]
        public async Task<IActionResult> StaffLogin(LoginInformation loginInformation)
        {
            // find 1 account with matching username in Account
            var ac = await _context.Account.SingleOrDefaultAsync(a =>
                    a.Username == loginInformation.Username);
            if (ac != null)
            {
                var isManager = ac.Id.StartsWith("MNG");
                var isAdmin = ac.Id.StartsWith("ADM");
                if (isManager || isAdmin)
                {
                    var roles = _context.AccountRoles.Where(acr => acr.AccountId == ac.Id);
                    // check matching password
                    if (ac.Password == PasswordHandle.GetInstance().EncryptPassword(loginInformation.Password, ac.Salt))
                    {
                        // check if account is logged in elsewhere
                        var cr = await _context.Credential.SingleOrDefaultAsync(c =>
                            c.OwnerId == ac.Id);
                        var accessToken = TokenHandle.GetInstance().GenerateToken();
                        if (cr != null) // if account has logged in
                        {
                            cr.AccessToken = accessToken;
                            // save token
                            _context.Credential.Update(cr);
                            await _context.SaveChangesAsync();
                            return Ok(accessToken);
                        }
                        // create new credential with AccountId
                        var firstCredential = new Credential
                        {
                            OwnerId = ac.Id,
                            AccessToken = accessToken
                        };
                        _context.Credential.Add(firstCredential);
                        await _context.SaveChangesAsync();
                        // save token
                        return Ok(accessToken);
                    }
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return new JsonResult(new ResponseError("UserName or Password is incorrect", (int)HttpStatusCode.Forbidden));
                }
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return new JsonResult(new ResponseError("Client Wrong", (int)HttpStatusCode.Forbidden));
            }
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return new JsonResult(new ResponseError("UserName or Password is incorrect", (int)HttpStatusCode.Forbidden));
        }

        // POST: api/Authentication/Logout
        [Route("Logout")]
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