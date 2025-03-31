using System.Transactions;
using Unistream_T4.Dto;

namespace Unistream_T4.Services.Abstractions
{
    public interface ITransactionService
     {
        Task<AddTransactionResponseDto> AddDebitTransaction(TransactionDto transactionDto);
        Task<AddTransactionResponseDto> AddCreditTransaction(TransactionDto transactionDto);
        Task<RevertTransactionResponseDto> RevertTransaction(Guid id);
        Task<ClientBalanceReponseDto> GetClientBalance(Guid id);
        //Task<bool> UpdateClientBalance(Guid id);
    }
}
