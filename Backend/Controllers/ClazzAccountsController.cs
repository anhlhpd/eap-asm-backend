﻿using System;
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
    public class ClazzAccountsController : ControllerBase
    {
        private readonly BackendContext _context;

        public ClazzAccountsController(BackendContext context)
        {
            _context = context;
        }

        // GET: api/ClazzAccounts/5
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetClazz([FromRoute] string studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clazz = await _context.ClazzAccount.FindAsync(studentId);

            if (clazz == null)
            {
                return NotFound();
            }
            return Ok(clazz);
        }

        // Manager: get all students of 1 class
        // GET: api/Subjects/Manager/GetAllStudentOneClazz
        [HttpGet("Manager/GetAllStudentOneClazz")]
        public IEnumerable<ClazzAccount> ManagerGetAllStudentOneClazz(string clazzId)
        {
            return _context.ClazzAccount.Where(ca => ca.ClazzId == clazzId);
        }

        // GET: api/ClazzAccounts
        [HttpGet]
        public IEnumerable<ClazzAccount> GetClazzAccount()
        {
            return _context.ClazzAccount;
        }

        // GET: api/ClazzAccounts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClazzAccount([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ClazzAccount = await _context.ClazzAccount.FindAsync(id);

            if (ClazzAccount == null)
            {
                return NotFound();
            }

            return Ok(ClazzAccount);
        }

        // PUT: api/ClazzAccounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClazzAccount([FromRoute] string id, [FromBody] ClazzAccount clazzAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != clazzAccount.ClazzId)
            {
                return BadRequest();
            }
            
            _context.Entry(clazzAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClazzAccountExists(id))
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

        // POST: api/ClazzAccounts
        [HttpPost]
        public async Task<IActionResult> PostClazzAccount([FromBody] ClazzAccount clazzAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _context.ClazzAccount.Add(clazzAccount);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClazzAccountExists(clazzAccount.ClazzId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetClazzAccount", new { id = clazzAccount.ClazzId }, clazzAccount);
        }

        // DELETE: api/ClazzAccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClazzAccount([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ClazzAccount = await _context.ClazzAccount.FindAsync(id);
            if (ClazzAccount == null)
            {
                return NotFound();
            }

            _context.ClazzAccount.Remove(ClazzAccount);
            await _context.SaveChangesAsync();

            return Ok(ClazzAccount);
        }

        private bool ClazzAccountExists(string id)
        {
            return _context.ClazzAccount.Any(e => e.ClazzId == id);
        }
    }
}