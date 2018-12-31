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
    public class AccountsController : ControllerBase
    {
        private readonly BackendContext _context;

        public AccountsController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/Accounts
        [HttpGet]
        public IEnumerable<Account> GetAccount()
        {
            return _context.Account;
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _context.Account.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount([FromRoute] string id, [FromBody] Account account)
        {
           if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != account.Id)
            {
                return BadRequest();
            }
            
            if (_context.Account.SingleOrDefault(a=>a.Id == account.Id) != null) // Kiem tra account update co ton tai khong
            {
                
                var currentAccount = await _context.Account.SingleOrDefaultAsync(a => a.Id == account.Id);
                string tokenHeader = Request.Headers["Authorization"];
                var token = tokenHeader.Replace("Basic ", "");
                var tokenUser = _context.Credential.SingleOrDefault(c => c.AccessToken == token);
                if (tokenUser.OwnerId == currentAccount.Id ||
                    _context.AccountRoles.SingleOrDefault(ar=>ar.AccountId == tokenUser.OwnerId).RoleId > _context.AccountRoles.SingleOrDefault(ar => ar.AccountId == currentAccount.Id).RoleId)
                {
                    _context.Entry(account).State = EntityState.Modified;
                    return new JsonResult(account);
                    //account.CreatedAt = currentAccount.CreatedAt;
                    currentAccount = account;
                    currentAccount.GeneralInformation = account.GeneralInformation;
                    _context.Account.Update(account);
                    return new JsonResult(account);

                }
            }
            return BadRequest();
            

            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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

        // POST: api/Accounts
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> PostAccount(GeneralInformation generalInformation)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return new JsonResult(generalInformation);
            //var salt = PasswordHandle.GetInstance().GenerateSalt();
            //account.Salt = salt;
            //var password = PasswordHandle.GetInstance().EncryptPassword(account.Password, account.Salt);
            //account.Password = password;
            //_context.Account.Add(account);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetAccount", new { id = account.Id }, account);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();

            return Ok(account);
        }

        private bool AccountExists(string id)
        {
            return _context.Account.Any(e => e.Id == id);
        }
    }
}