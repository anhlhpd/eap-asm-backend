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
    public class MarksController : ControllerBase
    {
        private readonly BackendContext _context;

        public MarksController(BackendContext context)
        {
            _context = context;
        }

        // Get all marks in 1 subject
        // GET: api/Marks/
        [Route("student")]
        [HttpGet]
        public async Task<IActionResult> StudentGetMarks()
        {
            string tokenHeader = Request.Headers["Authorization"];
            var token = tokenHeader.Replace("Basic ", "");
            var cr = _context.Credential.SingleOrDefault(c =>
                c.AccessToken == token);
            var accountRoles = _context.AccountRoles.Where(ar => ar.AccountId == cr.OwnerId).Include(ar => ar.Role);
            if (Request.Query.ContainsKey("SubjectId"))
            {
                var subjectId = Request.Query["SubjectId"].ToString();
                return Ok(_context.Mark.Where(m => m.AccountId == cr.OwnerId && m.SubjectId == subjectId)
                    .Include(m => m.Subject));
            }

            return Ok(_context.Mark.Where(m => m.AccountId == cr.OwnerId).Include(m => m.Subject));

        }

        [Route("Manager")]
        [HttpGet]
        public async Task<IActionResult> ManagerGetMarks()
        {
            bool isValid = (Request.Query.ContainsKey("ClassId") == Request.Query.ContainsKey("SubjectId")) &&
                           (Request.Query.ContainsKey("SubjectId") != Request.Query.ContainsKey("StudentId"));
            if (isValid)
            {
                if (Request.Query.ContainsKey("ClassId") && Request.Query.ContainsKey("SubjectId"))
                {
                    var classId = Request.Query["ClassId"].ToString();
                    var subjectId = Request.Query["SubjectId"].ToString();
                    var classAccounts = _context.ClazzAccount.Where(ca => ca.ClazzId == classId);

                    List<Mark> listMarks = new List<Mark>();
                    if (classAccounts.Any())
                    {
                        foreach (var classAccount in classAccounts)
                        {
                            var markAccountList = _context.Mark.Include(m => m.Subject).Include(m => m.Account)
                                .Where(m => m.AccountId == classAccount.AccountId && m.SubjectId == subjectId);
                            foreach (var mark in markAccountList)
                            {
                                listMarks.Add(mark);
                            }
                        }

                        return Ok(listMarks);
                    }

                    return NotFound();
                }

                if (Request.Query.ContainsKey("StudentId"))
                {
                    var studentId = Request.Query["StudentId"].ToString();
                    return Ok(_context.Mark.Where(m => m.AccountId == studentId).Include(m => m.Subject));
                }

                return NotFound();
            }

            return BadRequest();
        }

    }
}