using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Microsoft.AspNetCore.Cors;
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
        public async Task<IActionResult> GetAccount()
        {
            if (Request.Query.ContainsKey("RoleName"))
            {
                var roleName = Request.Query["RoleName"].ToString();
                var currentRole = await _context.Role.SingleOrDefaultAsync(r => r.Name == roleName);
                if (currentRole != null)
                {
                    var currentAccountRole = _context.AccountRoles.Where(ar => ar.RoleId == currentRole.Id).ToList();
                    if (currentAccountRole.Count > 0)
                    {
                        var listAccounts = await _context.Account.Include(a=>a.GeneralInformation).ToListAsync();
                        List<Account> listAccountsWithRole = new List<Account>();
                        foreach (var ac in currentAccountRole)
                        {

                            var currentAccount = listAccounts.SingleOrDefault(a=>a.Id == ac.AccountId);
                            listAccountsWithRole.Add(currentAccount);
                        }

                        if (listAccountsWithRole.Any())
                        {
                            if (Request.Query.ContainsKey("SearchKey"))
                            {
                                List<Account> listAccountsWithRoleAndSearchName = new List<Account>();
                                var searchKey = Request.Query["SearchKey"].ToString();
                                
                                foreach (var a in listAccountsWithRole)
                                {
                                    
                                    if (a.Id.Contains(searchKey)
                                        || a.Email.Contains(searchKey)
                                        || a.Username.Contains(searchKey)
                                        || a.GeneralInformation.FirstName.Contains(searchKey)
                                        || a.GeneralInformation.LastName.Contains(searchKey)
                                        || a.GeneralInformation.Phone.Contains(searchKey))
                                    {
                                        listAccountsWithRoleAndSearchName.Add(a);
                                    }
                                }
                                
                                if (listAccountsWithRoleAndSearchName.Any())
                                {
                                    return Ok(listAccountsWithRoleAndSearchName);
                                }

                                return NotFound();
                            }
                            return Ok(listAccountsWithRole);
                        }
                    }
                }
                return NotFound();
            }
            return Ok(_context.Account.Include(a => a.GeneralInformation));
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _context.Account.Include(a=>a.GeneralInformation).SingleOrDefaultAsync(a=>a.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        [EnableCors("AllowAny")]
        public async Task<IActionResult> PutAccount([FromRoute] string id, [FromBody] Account account)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != account.Id)
            {
                return BadRequest();
            }
            
            if ( await _context.Account.SingleOrDefaultAsync(a=>a.Id == account.Id) != null) // Kiem tra account update co ton tai khong
            {
                
                var currentAccount = await _context.Account.SingleOrDefaultAsync(a => a.Id == account.Id);
                string tokenHeader = Request.Headers["Authorization"];
                var token = tokenHeader.Replace("Basic ", "");
                var tokenUser = await _context.Credential.SingleOrDefaultAsync(c => c.AccessToken == token);
                if (tokenUser.OwnerId == currentAccount.Id 
                    ||
                    (await _context.AccountRoles.SingleOrDefaultAsync(ar=>ar.AccountId == tokenUser.OwnerId)).RoleId > (await _context.AccountRoles.SingleOrDefaultAsync(ar => ar.AccountId == currentAccount.Id)).RoleId ||
                    tokenUser.OwnerId == "ADMIN"
                    )
                {
                    if (account.Password == null)
                    {
                        account.Password = currentAccount.Password;
                        account.Salt = currentAccount.Salt;
                    }
                    else
                    {
                        if (PasswordHandle.GetInstance().EncryptPassword(account.Password,currentAccount.Salt) == currentAccount.Password) //Kiểm tra mật  khẩu có trùng với mật khẩu cũ không, nếu trùng thì trả về lỗi
                        {
                            return BadRequest(new ResponseError("New password do not same old password",400));
                        }
                        account.Salt = PasswordHandle.GetInstance().GenerateSalt();
                        account.Password = PasswordHandle.GetInstance().EncryptPassword(account.Password, account.Salt);
                        
                    }
                    account.UpdatedAt = DateTime.Now;
                    _context.Entry(account).State = EntityState.Modified;
                    _context.Entry(account.GeneralInformation).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(_context.Account.Include(a=>a.GeneralInformation).SingleOrDefault(a=>a.Id == account.Id));
                }
            }
            return BadRequest();

            
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
            if (account != null)
            {
                string tokenHeader = Request.Headers["Authorization"];
                var token = tokenHeader.Replace("Basic ", "");
                var tokenUser = await _context.Credential.SingleOrDefaultAsync(c => c.AccessToken == token);

                if (tokenUser.OwnerId == account.Id)//Không được xóa bản thân người dùng
                {
                    return BadRequest(new ResponseError("Cannot delete Self Account",400));
                }
                // người dùng cần quyền để xóa
                // fix ý nghĩa id càng to thì xóa được bịn id nhỏ 
               
                if ((await _context.AccountRoles.SingleOrDefaultAsync(ar => ar.AccountId == tokenUser.OwnerId)).RoleId < (await _context.AccountRoles.SingleOrDefaultAsync(ar => ar.AccountId == account.Id)).RoleId ||
                    tokenUser.OwnerId == "ADMIN"
                    )
                {
                    
                    account.UpdatedAt = DateTime.Now;
                    account.Status = AccountStatus.Deactive;
                    _context.Entry(account).State = EntityState.Modified;
                    //return new JsonResult(account);
                    await _context.SaveChangesAsync();
                    return Ok(account);
                }
            }
            return NotFound();
            
        }

        private bool AccountExists(string id)
        {
            return _context.Account.Any(e => e.Id == id);
        }
    }
}