using BankManagmentDB.Entity;
using BankManagmentDB;
using Microsoft.AspNetCore.Mvc;


namespace BankManagment.Controllers
{
    [Route("api/AccountType")]
    [ApiController]
    public class AccountTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("accounttypes")]
        public async Task<IActionResult> GetAccountTypes()
        {
            var accountTypes = await _unitOfWork.AccountTypesRepository.GetAllAsync();
            return Ok(accountTypes);
        }
        [HttpPost("accounttypes")]
        public async Task<IActionResult> CreateAccountType([FromBody] AccountType accountType)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _unitOfWork.AccountTypesRepository.AddAsync(accountType);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetAccountTypes), new { id = accountType.Id }, accountType);

        }
        [HttpPut("accounttypes/{id}")]
        public async Task<IActionResult> UpdateAccountType(Guid id, [FromBody] AccountType updatedAccountType)
        {
            if (!ModelState.IsValid || id != updatedAccountType.Id)
                return BadRequest();

            var existingAccountType = await _unitOfWork.AccountTypesRepository.GetByIdAsync(id);
            if (existingAccountType == null)
                return NotFound();

            existingAccountType.Name = updatedAccountType.Name;

            await _unitOfWork.AccountTypesRepository.UpdateAsync(existingAccountType);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("accounttypes/{id}")]
        public async Task<IActionResult> DeleteAccountType(Guid id)
        {
            var accountType = await _unitOfWork.AccountTypesRepository.GetByIdAsync(id);
            if (accountType == null)
                return NotFound();

            await _unitOfWork.AccountTypesRepository.DeleteAsync(accountType);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
