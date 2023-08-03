using BankManagmentDB.Entity;
using Microsoft.AspNetCore.Mvc;
using BankManagmentDB;

namespace BankManagment.Controllers
{
    [Route("api/PaymentMethod")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentMethodController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("paymentmethods")]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var paymentMethods = await _unitOfWork.PaymentMethodsRepository.GetAllAsync();
            return Ok(paymentMethods);
        }
        [HttpPost("paymentmethods")]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] PaymentMethod paymentMethod)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _unitOfWork.PaymentMethodsRepository.AddAsync(paymentMethod);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetPaymentMethods), new { id = paymentMethod.Id }, paymentMethod);
        }
        [HttpPut("paymentmethods/{id}")]
        public async Task<IActionResult> UpdatePaymentMethod(Guid id, [FromBody] PaymentMethod updatedPaymentMethod)
        {
            if (!ModelState.IsValid || id != updatedPaymentMethod.Id)
                return BadRequest();

            var existingPaymentMethod = await _unitOfWork.PaymentMethodsRepository.GetByIdAsync(id);
            if (existingPaymentMethod == null)
                return NotFound();

            existingPaymentMethod.Name = updatedPaymentMethod.Name;

            await _unitOfWork.PaymentMethodsRepository.UpdateAsync(existingPaymentMethod);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
        [HttpDelete("paymentmethods/{id}")]
        public async Task<IActionResult> DeletePaymentMethod(Guid id)
        {
            var paymentMethod = await _unitOfWork.PaymentMethodsRepository.GetByIdAsync(id);
            if (paymentMethod == null)
                return NotFound();

            await _unitOfWork.PaymentMethodsRepository.DeleteAsync(paymentMethod);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
