using BankManagmentDB.Entity;
using BankManagmentDB;
using Microsoft.AspNetCore.Mvc;


namespace BankManagment.Controllers
{
    [Route("api/BankTransactions")]
    [ApiController]
 public class BankTransactionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BankTransactionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("banktransactions")]
        public async Task<IActionResult> GetBankTransactions()
        {
            var bankTransactions = await _unitOfWork.BankTransactionsRepository.GetAllAsync();
            return Ok(bankTransactions);
        }
        [HttpPost("banktransactions")]
        public async Task<IActionResult> CreateBankTransaction([FromBody] BankTransaction bankTransaction)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _unitOfWork.BankTransactionsRepository.AddAsync(bankTransaction);
            await _unitOfWork.SaveAsync();

            // Check if the BankTransaction is of type "Bank Interest" or "Bank Charges"
            if (bankTransaction.Category == "Bank Interest" || bankTransaction.Category == "Bank Charges")
            {

                var bankAccountPosting = new BankAccountPosting
                {
                    TransactionPersonFirstName = bankTransaction.TransactionPersonFirstName,
                    TransactionPersonMiddleName = bankTransaction.TransactionPersonMiddleName,
                    TransactionPersonLastName = bankTransaction.TransactionPersonLastName,
                    TransactionType = bankTransaction.TransactionType,
                    Category = bankTransaction.Category,
                    Amount = bankTransaction.Amount,
                    TransactionDate = bankTransaction.TransactionDate,
                    PaymentMethodId = bankTransaction.PaymentMethodID,
                    BankAccountId = bankTransaction.BankAccountID
                };

                await _unitOfWork.BankAccountPostingsRepository.AddAsync(bankAccountPosting);
                await _unitOfWork.SaveAsync();
            }

            // Update the BankAccount's TotalBalance based on the transaction
            var bankAccount = await _unitOfWork.BankAccountsRepository.GetByIdAsync(bankTransaction.BankAccountID);
            if (bankAccount == null)
                return NotFound();

            if (bankTransaction.TransactionType == "Credit")
            {
                bankAccount.TotalBalance += bankTransaction.Amount;
            }
            else if (bankTransaction.TransactionType == "Debit")
            {
                bankAccount.TotalBalance -= bankTransaction.Amount;
            }


            await _unitOfWork.BankAccountsRepository.UpdateAsync(bankAccount);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetBankTransactions), new { id = bankTransaction.Id }, bankTransaction);
        }
        [HttpPut("banktransactions/{id}")]
        public async Task<IActionResult> UpdateBankTransaction(Guid id, [FromBody] BankTransaction updatedBankTransaction)
        {
            if (!ModelState.IsValid || id != updatedBankTransaction.Id)
                return BadRequest();

            var existingBankTransaction = await _unitOfWork.BankTransactionsRepository.GetByIdAsync(id);
            if (existingBankTransaction == null)
                return NotFound();

            existingBankTransaction.TransactionPersonFirstName = updatedBankTransaction.TransactionPersonFirstName;
            existingBankTransaction.TransactionPersonMiddleName = updatedBankTransaction.TransactionPersonMiddleName;
            existingBankTransaction.TransactionPersonLastName = updatedBankTransaction.TransactionPersonLastName;
            existingBankTransaction.TransactionType = updatedBankTransaction.TransactionType;
            existingBankTransaction.Category = updatedBankTransaction.Category;
            existingBankTransaction.Amount = updatedBankTransaction.Amount;
            existingBankTransaction.TransactionDate = updatedBankTransaction.TransactionDate;
            // existingBankTransaction.PaymentMethod_FK = updatedBankTransaction.PaymentMethod_FK;
            // existingBankTransaction.BankAccount_FK = updatedBankTransaction.BankAccount_FK;

            await _unitOfWork.BankTransactionsRepository.UpdateAsync(existingBankTransaction);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
        [HttpDelete("banktransactions/{id}")]
        public async Task<IActionResult> DeleteBankTransaction(Guid id)
        {
            var bankTransaction = await _unitOfWork.BankTransactionsRepository.GetByIdAsync(id);
            if (bankTransaction == null)
                return NotFound();

            await _unitOfWork.BankTransactionsRepository.DeleteAsync(bankTransaction);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
