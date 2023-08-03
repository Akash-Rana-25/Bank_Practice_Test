using BankManagmentDB.Entity;
using BankManagmentDB;
using Microsoft.AspNetCore.Mvc;

namespace BankManagment.Controllers
{
    [Route("api/BankAccount")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("bankaccounts")]
        public async Task<IActionResult> GetBankAccounts()
        {
            var bankAccounts = await _unitOfWork.BankAccountsRepository.GetAllAsync();
            return Ok(bankAccounts);
        }

        // POST: api/bank/bankaccounts
        [HttpPost("bankaccounts")]
        public async Task<IActionResult> CreateBankAccount([FromBody] BankAccount bankAccount)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _unitOfWork.BankAccountsRepository.AddAsync(bankAccount);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetBankAccounts), new { id = bankAccount.Id }, bankAccount);
        }
        [HttpPut("bankaccounts/{id}")]
        public async Task<IActionResult> UpdateBankAccount(Guid id, [FromBody] BankAccount updatedBankAccount)
        {
            if (!ModelState.IsValid || id != updatedBankAccount.Id)
                return BadRequest();

            var existingBankAccount = await _unitOfWork.BankAccountsRepository.GetByIdAsync(id);
            if (existingBankAccount == null)
                return NotFound();

            existingBankAccount.FirstName = updatedBankAccount.FirstName;
            existingBankAccount.MiddleName = updatedBankAccount.MiddleName;
            existingBankAccount.LastName = updatedBankAccount.LastName;
            // existingBankAccount.AccountNumber = updatedBankAccount.AccountNumber;    
            // existingBankAccount.OpeningDate = updatedBankAccount.OpeningDate;
            existingBankAccount.ClosingDate = updatedBankAccount.ClosingDate;
            // existingBankAccount.AccountType_FK = updatedBankAccount.AccountType_FK;
            // existingBankAccount.TotalBalance = updatedBankAccount.TotalBalance;

            await _unitOfWork.BankAccountsRepository.UpdateAsync(existingBankAccount);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
        [HttpDelete("bankaccounts/{id}")]
        public async Task<IActionResult> DeleteBankAccount(Guid id)
        {
            var bankAccount = await _unitOfWork.BankAccountsRepository.GetByIdAsync(id);
            if (bankAccount == null)
                return NotFound();

            await _unitOfWork.BankAccountsRepository.DeleteAsync(bankAccount);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }

}
