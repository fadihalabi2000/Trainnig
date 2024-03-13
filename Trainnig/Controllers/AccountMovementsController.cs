using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainnigApI.Abstration.Enum;
using TrainnigApI.Data;
using TrainnigApI.Model;
using TrainnigApI.View;

namespace TrainnigApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountMovementsController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly service.IBaseService<AccountMovement> baseService;
        public AccountMovementsController(AppDBContext context, 
                                   service.IBaseService<AccountMovement> baseService)
        {
            _context = context;
            this.baseService = baseService;
        }

        // GET: api/AccountMovements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountMovement>>> GetaccountMovements()
        {
            return await _context.accountMovements.ToListAsync();
        }

        // GET: api/AccountMovements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountMovement>> GetAccountMovement(int id)
        {
            var accountMovement = await _context.accountMovements.FindAsync(id);

            if (accountMovement == null)
            {
                return NotFound();
            }

            return accountMovement;
        }

        // PUT: api/AccountMovements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountMovement(int id, AccountMovement accountMovement)
        {
            if (id != accountMovement.ID)
            {
                return BadRequest();
            }

            _context.Entry(accountMovement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountMovementExists(id))
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

        // POST: api/AccountMovements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<AccountMovementView>> PostAccountMovement(AccountMovementView accountMovement)
        //{
        //    //_context.accountMovements.Add(accountMovement);
        //    //await _context.SaveChangesAsync();
        //    try
        //    {
        //        var lastAccountMovementId = _context.accountMovements
        //                       .OrderByDescending(b => b.ID)
        //                       .Select(b => b.ID)
        //                       .FirstOrDefault();



        //        // return NotFound($"This room id: {id} does not exist");
        //        AccountMovement accountMovementforAdd = new AccountMovement()
        //        {
        //            AccountId = accountMovement.AccountId,
        //            ReservationId = accountMovement.ReservationId,
        //            MovementDate = accountMovement.MovementDate,
        //            MovementValue = accountMovement.MovementValue,
        //            AccountStatement= accountMovement.AccountStatement,
        //            Status = accountMovement.Status,
        //        };
        //            try
        //            {

        //                await _context.accountMovements.AddAsync(accountMovementforAdd);
        //                await _context.SaveChangesAsync();
        //                if (lastAccountMovementId >= 0)
        //                {
        //                lastAccountMovementId += 1;
        //                    Response.Headers.Append("AccountMovement-ID", lastAccountMovementId.ToString());
        //                }
        //             return Ok(accountMovementforAdd);
        //          //  return CreatedAtAction("GetAccountMovement", lastAccountMovementId, accountMovement);
        //        }
        //            catch (Exception)
        //            { return NotFound("fialed to add accountMovement soory"); }

        //    }


        //    catch (Exception)
        //    {
        //        return Conflict("sorry try agin");
        //    }


        //}

        // DELETE: api/AccountMovements/5

        //

        [HttpPost]
        public async Task<ActionResult<AccountMovementView>> PostAccountMovement(AccountMovementView accountMovement)
        {
            try
            {
                var lastAccountMovementId = _context.accountMovements
                    .OrderByDescending(b => b.ID)
                    .Select(b => b.ID)
                    .FirstOrDefault();

                // Return NotFound($"This room id: {id} does not exist");
                AccountMovement accountMovementforAdd = new AccountMovement()
                {
                    AccountId = accountMovement.AccountId,
                    ReservationId = accountMovement.ReservationId,
                    MovementDate = accountMovement.MovementDate,
                    MovementValue = accountMovement.MovementValue,
                    AccountStatement = accountMovement.AccountStatement,
                    Status = accountMovement.Status,
                };

                try
                {
                    // Retrieve the current balance of the account
                    var account = await _context.accounts.FindAsync(accountMovement.AccountId);

                    if (account == null)
                    {
                        return NotFound("Account not found");
                    }

                    // Update the account balance based on the movement type
                    if (accountMovement.Status == MovementStatus.Positive)
                    {
                        account.TotalBalance += accountMovement.MovementValue;
                    }
                    else if (accountMovement.Status == MovementStatus.Negative)
                    {
                        account.TotalBalance -= accountMovement.MovementValue;
                    }

                    // Save changes to the account
                    _context.accounts.Update(account);
                    await _context.SaveChangesAsync();

                    // Set the movement value based on the movement type
                    accountMovementforAdd.MovementValue = (accountMovement.Status == MovementStatus.Negative) ?
                        -accountMovement.MovementValue : accountMovement.MovementValue;

                    // Add the movement to the AccountMovements table
                    await _context.accountMovements.AddAsync(accountMovementforAdd);
                    await _context.SaveChangesAsync();

                    if (lastAccountMovementId >= 0)
                    {
                        lastAccountMovementId += 1;
                        Response.Headers.Append("AccountMovement-ID", lastAccountMovementId.ToString());
                    }

                    return Ok(accountMovementforAdd);
                }
                catch (Exception)
                {
                    return NotFound("Failed to add accountMovement. Sorry.");
                }
            }
            catch (Exception)
            {
                return Conflict("Sorry, try again.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountMovement(int id)
        {
            var accountMovement = await _context.accountMovements.FindAsync(id);
            if (accountMovement == null)
            {
                return NotFound();
            }

            _context.accountMovements.Remove(accountMovement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountMovementExists(int id)
        {
            return _context.accountMovements.Any(e => e.ID == id);
        }
    }
}
