using System.Threading.Tasks;
using BankingApi.EventReceiver.Contracts;

namespace BankingApi.EventReceiver
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankingApiDbContext _context;

        public IBankAccountRepository BankAccounts { get; }
        public ITransactionRepository Transactions { get; }

        public UnitOfWork(BankingApiDbContext context,
                          IBankAccountRepository bankAccountRepository,
                          ITransactionRepository transactionRepository)
        {
            _context = context;
            BankAccounts = bankAccountRepository;
            Transactions = transactionRepository;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
