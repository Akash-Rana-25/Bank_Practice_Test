using BankManagmentDB.Entity;
using Microsoft.EntityFrameworkCore;


namespace BankManagmentDB.Context
{
    public class ApplicationDbContext : DbContext
    {

       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
       


        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<BankAccountPosting> BankAccountPostings { get; set; }

    

    }
}
