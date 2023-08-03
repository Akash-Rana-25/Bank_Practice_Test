using System.ComponentModel.DataAnnotations;

namespace BankManagmentDB.Entity
{
    public class BankAccountPosting
    {
        public Guid Id { get; set; }
        [Required]
        public string TransactionPersonFirstName { get; set; }
        public string TransactionPersonMiddleName { get; set; }
        [Required]
        public string TransactionPersonLastName { get; set; }
        public string TransactionType { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid PaymentMethodId { get; set; }
        public Guid BankAccountId { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual BankAccount BankAccount { get; set; }

    }
}
