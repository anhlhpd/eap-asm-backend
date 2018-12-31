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
    public class AccountRolesController : ControllerBase
    {
        private readonly BackendContext _context;

        public AccountRolesController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/AccountRoles
        [HttpGet]
        public IEnumerable<AccountRole> GetAccountRoles()
        {
            return _context.AccountRoles;
        }

        // GET: api/AccountRoles/5
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetListStudent([FromRoute] int roleId)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountRoles = _context.AccountRoles.Where(ar => ar.RoleId == roleId);

            if (accountRoles == null || accountRoles.Count() == 0)
            {
                return NotFound();
            }
            var listStudents = new List<GeneralInformation>();
            foreach (var ar in accountRoles)
            {
                var acId = ar.AccountId;
                GeneralInformation listStudent = _context.GeneralInformation.SingleOrDefault(st => st.AccountId == acId);
                if (listStudent == null)
                {
                    return NotFound();
                }
                listStudents.Add(listStudent);
            }
            if(listStudents == null || listStudents.Count() == 0)
            {
                return NotFound("yes");
            }
            return Ok(listStudents);
        }

        // PUT: api/AccountRoles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountRole([FromRoute] int id, [FromBody] AccountRole accountRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != accountRole.RoleId)
            {
                return BadRequest();
            }

            _context.Entry(accountRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountRoleExists(id))
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

        // POST: api/AccountRoles
        [HttpPost]
        public async Task<IActionResult> PostAccountRole([FromBody] AccountRole accountRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AccountRoles.Add(accountRole);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AccountRoleExists(accountRole.RoleId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAccountRole", new { id = accountRole.RoleId }, accountRole);
        }

        // DELETE: api/AccountRoles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountRole([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountRole = await _context.AccountRoles.FindAsync(id);
            if (accountRole == null)
            {
                return NotFound();
            }

            _context.AccountRoles.Remove(accountRole);
            await _context.SaveChangesAsync();

            return Ok(accountRole);
        }

        private bool AccountRoleExists(int id)
        {
            return _context.AccountRoles.Any(e => e.RoleId == id);
        }
    }
}