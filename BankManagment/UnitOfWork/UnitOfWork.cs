using BankManagmentDB.Context;
using BankManagmentDB.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankManagmentDB
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private IRepository<AccountType> _accountTypesRepository;
        public IRepository<AccountType> AccountTypesRepository =>
            _accountTypesRepository ??= new Repository<AccountType>(_context);

        private IRepository<PaymentMethod> _paymentMethodsRepository;
        public IRepository<PaymentMethod> PaymentMethodsRepository =>
            _paymentMethodsRepository ??= new Repository<PaymentMethod>(_context);

        private IRepository<BankAccount> _bankAccountsRepository;
        public IRepository<BankAccount> BankAccountsRepository =>
            _bankAccountsRepository ??= new Repository<BankAccount>(_context);

        private IRepository<BankTransaction> _bankTransactionsRepository;
        public IRepository<BankTransaction> BankTransactionsRepository =>
            _bankTransactionsRepository ??= new Repository<BankTransaction>(_context);

        private IRepository<BankAccountPosting> _bankAccountPostingsRepository;
        public IRepository<BankAccountPosting> BankAccountPostingsRepository =>
            _bankAccountPostingsRepository ??= new Repository<BankAccountPosting>(_context);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
