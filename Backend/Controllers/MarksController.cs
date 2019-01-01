
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

        // Student: get all marks in 1 subject
        // GET: api/Marks/StudentGetMarksOneSubject
        [HttpGet("StudentGetMarksOneSubject")]
        public IEnumerable<Mark> StudentGetMarksOneSubject(string subjectId, HttpContext context)
        {
            string tokenHeader = context.Request.Headers["Authorization"];
            var token = tokenHeader.Replace("Basic ", "");
            var cr =  _context.Credential.SingleOrDefault(c =>
                    c.AccessToken == token);
            return _context.Mark.Where(m => m.AccountId == cr.OwnerId && m.SubjectId == subjectId);
        }

        // Student: get all marks in all subject
        // GET: api/Marks/StudentGetMarksAllSubject
        [HttpGet("StudentGetMarksAllSubject")]
        public IEnumerable<Mark> StudentGetMarksAllSubject(HttpContext context)
        {
            // Call method StudentGetAllSubject from SubjectController ?
            SubjectsController ctrl = new SubjectsController();
            ctrl.ControllerContext = ControllerContext;
            IEnumerable<ClazzSubject> listClazzSubjects = ctrl.StudentGetAllSubject(context);
            IEnumerable<Mark> listMarks = null;
            foreach (var clazzSubject in listClazzSubjects)
            {
                IEnumerable<Mark> listAllMarksOneSubject = StudentGetMarksOneSubject(clazzSubject.SubjectId, context);
                foreach(var mark in listAllMarksOneSubject)
                {
                    listMarks.Append(mark);
                }
            }
            return listMarks;
        }

        // Manager: get all marks in 1 subject from 1 student
        // GET: api/Marks/ManagerGetMarksOneSubjectOneStudent
        [HttpGet("Manager/GetMarksOneSubjectOneStudent")]
        public IEnumerable<Mark> ManagerGetMarksOneSubjectOneStudent(string subjectId, string studentId)
        {
            return _context.Mark.Where(m => m.AccountId == studentId && m.SubjectId == subjectId);
        }

        // Manager: get all marks in all subject from 1 student
        // GET: api/Marks/ManagerGetMarksAllSubjectOneStudent
        [HttpGet("Manager/GetMarksAllSubjectOneStudent")]
        public IEnumerable<Mark> ManagerGetMarksAllSubjectOneStudent(string studentId)
        {
            // Call method StudentGetAllSubject from SubjectController ?
            SubjectsController ctrl = new SubjectsController();
            ctrl.ControllerContext = ControllerContext;
            IEnumerable<ClazzSubject> listClazzSubjects = ctrl.ManagerGetAllSubjectOneStudent(studentId);
            IEnumerable<Mark> listMarks = null;
            foreach (var clazzSubject in listClazzSubjects)
            {
                IEnumerable<Mark> listAllMarksOneSubject = ManagerGetMarksOneSubjectOneStudent(clazzSubject.SubjectId, studentId);
                foreach (var mark in listAllMarksOneSubject)
                {
                    listMarks.Append(mark);
                }
            }
            return listMarks;
        }

        // Manager: get all marks in 1 subject from all student
        // GET: api/Marks/GetMarksOneSubjectAllStudent
        [HttpGet("Manager/GetMarksOneSubjectAllStudent")]
        public IEnumerable<Mark> ManagerGetMarksOneSubjectAllStudent(string subjectId)
        {
            // Call method ManagerGetAllStudentOneSubject from ClazzAccountsController ?
            ClazzAccountsController ctrl = new ClazzAccountsController();
            ctrl.ControllerContext = ControllerContext;
            IEnumerable<ClazzAccount> listClazzAccounts = ctrl.ManagerGetAllStudentOneSubject(subjectId);
            IEnumerable<Mark> listMarks = null;
            foreach (var clazzAccount in listClazzAccounts)
            {
                IEnumerable<Mark> listAllMarksOneStudent = ManagerGetMarksOneSubjectOneStudent(subjectId, clazzAccount.AccountId);
                foreach (var mark in listAllMarksOneStudent)
                {
                    listMarks.Append(mark);
                }
            }
            return listMarks;
        }

        // Manager: get all marks in 1 subject from all student in 1 class
        // GET: api/Marks/GetMarksOneSubjectAllStudentOneClazz
        [HttpGet("Manager/GetMarksOneSubjectAllStudentOneClazz")]
        public IEnumerable<Mark> ManagerGetMarksOneSubjectAllStudentOneClazz(string subjectId, string clazzId)
        {
            // Call method ManagerGetAllStudentOneSubject from ClazzAccountsController ?
            ClazzAccountsController ctrl = new ClazzAccountsController();
            ctrl.ControllerContext = ControllerContext;
            IEnumerable<ClazzAccount> listClazzAccounts = ctrl.ManagerGetAllStudentOneClazz(clazzId);
            IEnumerable<Mark> listMarks = null;
            foreach (var clazzAccount in listClazzAccounts)
            {
                IEnumerable<Mark> listAllMarksOneStudent = ManagerGetMarksOneSubjectOneStudent(subjectId, clazzAccount.AccountId);
                foreach (var mark in listAllMarksOneStudent)
                {
                    listMarks.Append(mark);
                }
            }
            return listMarks;
        }

        // GET: api/Marks
        [HttpGet]
        public IEnumerable<Mark> GetMark()
        {
            return _context.Mark;
        }

        // GET: api/Marks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMark([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mark = await _context.Mark.FindAsync(id);

            if (mark == null)
            {
                return NotFound();
            }

            return Ok(mark);
        }

        // PUT: api/Marks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMark([FromRoute] long id, [FromBody] Mark mark)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mark.Id)
            {
                return BadRequest();
            }

            _context.Entry(mark).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarkExists(id))
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

        // POST: api/Marks
        [HttpPost]
        public async Task<IActionResult> PostMark([FromBody] Mark mark)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Mark.Add(mark);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMark", new { id = mark.Id }, mark);
        }

        // DELETE: api/Marks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMark([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mark = await _context.Mark.FindAsync(id);
            if (mark == null)
            {
                return NotFound();
            }

            _context.Mark.Remove(mark);
            await _context.SaveChangesAsync();

            return Ok(mark);
        }

        private bool MarkExists(long id)
        {
            return _context.Mark.Any(e => e.Id == id);
        }
    }
}