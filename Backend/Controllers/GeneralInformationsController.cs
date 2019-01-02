using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Microsoft.AspNetCore.Routing;

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
        //

        [HttpPost("{accountType}")]
        public async Task<IActionResult> PostGeneralInformationInformation([FromRoute] string accountType, [FromBody] GeneralInformation generalInformation)
        {
            string[] allType = { "STU", "MNG", "ADM" };
            if (!allType.Contains(accountType))
            {
                return BadRequest();
            }

            var numb = await _context.Account.CountAsync(a => a.Id.Contains(accountType)) + 1;
            //Random string
            string RandomString(int length, string method)
            {
                string  chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                if (method == "taging")
                {
                    chars = "0123456789";
                }
                
                Random random = new Random();
                return new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

            }
            //Remove Unsign 
            string convertToUnSign3(string s)
            {
                Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
                string temp = s.Normalize(NormalizationForm.FormD);
                return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            }

            
            
            
            //Create userName and ID
            var str = generalInformation.LastName.Split(" ");
            string userName = generalInformation.FirstName;
            foreach (var item in str)
            {
                if (item.Any())
                {
                    userName += item[0];
                }
            }

            while (true)
            {
                string taging = RandomString(4, "taging");
                taging += convertToUnSign3(generalInformation.FirstName[0].ToString());
                generalInformation.AccountId = accountType + taging;
                if ((await _context.Account.FindAsync(generalInformation.AccountId)) == null)
                {
                    userName = convertToUnSign3(userName) + taging;
                    break;
                }
            }

            var salt = SecurityHandle.PasswordHandle.GetInstance().GenerateSalt();
            // Create PW
            var password = RandomString(8,"PW");
            

            Account account = new Account()
            {
                Id = generalInformation.AccountId,
                Username = userName.ToLower(),
                Email = userName.ToLower() + generalInformation.AccountId.ToLower() + "@gmail.com",
                Salt = salt,
                Password = SecurityHandle.PasswordHandle.GetInstance().EncryptPassword(password, salt)
            };
            _context.Account.Add(account);
            _context.GeneralInformation.Add(generalInformation);
            AccountRole ar = new AccountRole()
            {
                AccountId = account.Id,
                RoleId = (accountType == "STU" ? 3 :(accountType == "MNG" ? 2 : 1))
            };
            _context.AccountRoles.Add(ar);
            LoginInformation responceInformation = new LoginInformation()
            {
                Username = account.Username,
                Password = password,
                ClientId = accountType
            };
            try
            {
                
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException e)
            {
                if (GeneralInformationExists(generalInformation.AccountId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    if (e.InnerException.Message.Contains("Phone"))
                    {
                        return BadRequest("Số điện thoại không hợp lệ hoặc đã tồn tại.");
                    }

                    return BadRequest(e.InnerException.Message);
                }
            }


            return Created("", responceInformation);
        }
    }
}