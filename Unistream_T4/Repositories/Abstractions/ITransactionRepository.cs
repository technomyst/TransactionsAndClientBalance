using Unistream_T4.Models.Transactions;

namespace Unistream_T4.Repositories.Abstractions
{
    public interface ITransactionRepository
    {
        Task<CreditTransaction> AddCreditTransaction(CreditTransaction transaction);
        Task<DebitTransaction> AddDebitTransaction(DebitTransaction transaction);
        Task<Transaction> RevertTransaction(Transaction transaction);
        Task<Transaction?> GetTransactionById(Guid Id);
        Task<ClientBalance?> GetClientBalanceById(Guid Id);
    }
}
