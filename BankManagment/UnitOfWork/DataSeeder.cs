using BankManagmentDB.Context;
using BankManagmentDB.Entity;
using Microsoft.Extensions.Configuration;


namespace BankManagmentDB
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;



        public DataSeeder(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public void SeedData()
        {

            Guid firstAccount= Guid.NewGuid();
            var numberOfBankAccountRecords = _configuration["AppSettings:NumberOfBankAccountRecords"];
            var numberOfBankTransactionRecords = _configuration["AppSettings:NumberOfBankTransactionRecords"];




            if (!_dbContext.AccountTypes.Any())
            {
                
                _dbContext.AccountTypes.AddRange(new[]
                {
                new AccountType { Id = firstAccount, Name = "Liability" },
                new AccountType { Id = Guid.NewGuid(), Name = "Asset" }
                

            }
                );
                _dbContext.SaveChanges();

            }

            if (!_dbContext.PaymentMethods.Any())
            {
                
                _dbContext.PaymentMethods.AddRange(new[]
                {
                new PaymentMethod { Id = Guid.NewGuid(), Name = "Cash" },
                new PaymentMethod { Id = Guid.NewGuid(), Name = "Cheque" },
                new PaymentMethod { Id = Guid.NewGuid(), Name = "NEFT" },
                new PaymentMethod { Id = Guid.NewGuid(), Name = "RTGS" },
                new PaymentMethod { Id = Guid.NewGuid(), Name = "Other" }
            });
                _dbContext.SaveChanges();

            }

            if (!_dbContext.BankAccounts.Any())
            {

                if (int.TryParse(numberOfBankAccountRecords, out int numberOfBankAccountRecordsInt))
                {


                    for (int i = 0; i < numberOfBankAccountRecordsInt; i++)
                    {
                        _dbContext.BankAccounts.Add(new BankAccount
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "Akash",
                            LastName = "Rana",
                            AccountNumber = GenerateRandomAccountNumber(),
                            OpeningDate = DateTime.Now.AddDays(-i),
                            AccountTypeId = firstAccount,
                            TotalBalance = 1000
                        });
                    }
                    _dbContext.SaveChanges();

                }
            }


            if (!_dbContext.BankTransactions.Any())
            {

                var paymentMethods = _dbContext.PaymentMethods.ToList();
                var bankAccounts = _dbContext.BankAccounts.ToList();
                var random = new Random();
                if (int.TryParse(numberOfBankTransactionRecords, out int numberOfBankTransactionRecordsInt) && numberOfBankTransactionRecordsInt > 0)
                {


                    for (int i = 0; i < numberOfBankTransactionRecordsInt; i++)
                    {
                        var bankAccount = bankAccounts[random.Next(bankAccounts.Count)];
                        var paymentMethod = paymentMethods[random.Next(paymentMethods.Count)];

                        _dbContext.BankTransactions.Add(new BankTransaction
                        {
                            Id = Guid.NewGuid(),
                            TransactionPersonFirstName = "Akash",
                            TransactionPersonLastName = "Rana",
                            TransactionType = random.Next(2) == 0 ? "Credit" : "Debit",
                            Category = GetRandomCategory(random),
                            Amount = (decimal)random.NextDouble() * 1000,
                            TransactionDate = DateTime.Now.AddDays(-i),
                            PaymentMethodID = paymentMethod.Id,
                            BankAccountID = bankAccount.Id
                        });
                    }
                    _dbContext.SaveChanges();
                }
            }


        }

        private string GenerateRandomAccountNumber()
        {

            var random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }

        private string GetRandomCategory(Random random)
        {

            var categories = new[] { "Opening Balance", "Bank Interest", "Bank Charges", "Normal Transactions" };
            return categories[random.Next(categories.Length)];
        }
    }

}
