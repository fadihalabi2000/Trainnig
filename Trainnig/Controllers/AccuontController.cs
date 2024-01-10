using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainnigApI.Data;
using TrainnigApI.Model;
using TrainnigApI.service;
using TrainnigApI.View;

namespace TrainnigApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccuontController : ControllerBase
    {
        private readonly ILogger<AccuontController> _logger;
        private readonly service.IBaseService<Account> baseService;
        public AccuontController( ILogger<AccuontController> logger, IBaseService<Account> baseService)
        {
           
            _logger = logger;
            this.baseService = baseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAccounts()
        {
            try
            {
                var accounts = await this.baseService.GetAllAsync();

                if (!accounts.Any())
                {
                    return NotFound("There are no accounts");
                }

                return Ok(accounts);
            }
            catch
            {
                return BadRequest("Something went wrong in the request, please try again.");
            }
        }

        [HttpGet("{id}", Name = "GetAccuontByID")]
        public async Task<ActionResult<Account>> GetAccountById(int id)
        {

            var accountsById = await this.baseService.GetByIdAsync(id);


            try
            {
                if (accountsById != null)
                {
                    return Ok(accountsById);
                   
                }
                else
                {
                    return NotFound($"The Account id {id} does not exist");
                }

               
            }
            catch
            {
                return BadRequest("try agin something wong in request");
            }
        }
        [HttpPost]
        public async Task<ActionResult<AccuontView>> PostCenter(AccuontView accuontView)
        {
            try
            {   
               
                var accounts = await this.baseService.GetAllAsync();
                var lastAccountId=accounts.OrderByDescending(b => b.ID)
                                          .Select(b => b.ID)
                                          .FirstOrDefault();
                Account account = new Account()
                {
                    AccountName = accuontView.AccountName,
                    TotalBalance = accuontView.TotalBalance,
                };
                await this.baseService.AddAsync(account);
               // await _context.SaveChangesAsync();
                if (lastAccountId >= 0)
                {
                    lastAccountId += 1;
                    Response.Headers.Append($"Account-ID", lastAccountId.ToString());
                }
                return Ok("saccessfuly add Account");
            }
            catch (Exception)
            {
                return Conflict("sorry  add try agin");
            }
        }
       
        [HttpDelete("{id}", Name = "DeleteAccountByID")]
        public async Task<IActionResult> DeleteAccountByID(int id)
        {
            try
            {
                var deletedAccount = await this.baseService.GetByIdAsync(id);

                if (deletedAccount == null)
                {
                    return NotFound("The account does not exist");
                }
                else {
                    await this.baseService.DeleteAsync(id);

                    return Ok($"Deleted successfully account id {deletedAccount.ID}");
                }
                
            }
            catch (Exception ex)
            {
                return Conflict(ex.ToString());
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccountByID(int id,AccuontView accuontView)
        {
            try
            {
                var updateAccount = await this.baseService.GetByIdAsync(id);

                if (updateAccount == null)
                {
                    return NotFound($"The account id {id} does not exist");
                }
                else {
                    updateAccount.AccountName = accuontView.AccountName;
                    updateAccount.TotalBalance = accuontView.TotalBalance;
                    await this.baseService.UpdateAsync(updateAccount);
                    Response.Headers.Append($" updatedAccountName to:", (updateAccount.AccountName));
                    return Ok($"update successfully account id( {updateAccount.ID} )" +
                                    $"AccountName:{updateAccount.AccountName}" +
                                    $"TotalBalance:{updateAccount.TotalBalance}");
                }
               
            }
            catch (Exception )
            {
                return BadRequest("Something went wrong in " +
                         " the request, please try again.");
            }
        }

    }
}
