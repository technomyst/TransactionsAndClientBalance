using Microsoft.EntityFrameworkCore;
using Unistream_T4.Dto;
using Unistream_T4.Exceptions;
using Unistream_T4.Models.Abstractions;
using Unistream_T4.Models.Transactions;
using Unistream_T4.Repositories.Abstractions;

namespace Unistream_T4.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDbContext _dbContext;

        public TransactionRepository(TransactionDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public async Task<CreditTransaction> AddCreditTransaction(CreditTransaction transaction)
        {
            //Начинаем транзакцию БД
            await using var EfTransaction = await _dbContext.Database.BeginTransactionAsync();

            //Получаем баланс клиента
            var clientBalance = await _dbContext.ClientBalances.FirstOrDefaultAsync(x => x.ClientId == transaction.ClientId);
            
            decimal currentBalance = clientBalance?.Balance ?? 0m;

            //вычисляем баланс клиента с учетом транзакции

            var newBalance = transaction.CalculateBalance(currentBalance);

            transaction.ClientBalanceAfterAddTransaction = newBalance;
            transaction.InsertDateTime = DateTime.UtcNow;

            //добавляем транзакцию в базу
            await _dbContext.Transactions.AddAsync(transaction);
            
            if (clientBalance == null)
            {
                var newClientBalance = new ClientBalance()
                {
                    ClientId = transaction.ClientId,
                    Balance = newBalance,
                    LastCalculationDateTime = DateTime.UtcNow
                };
                await _dbContext.ClientBalances.AddAsync(newClientBalance);
                clientBalance = newClientBalance;
            }
            else
            {
                clientBalance.Balance = newBalance;
                clientBalance.LastCalculationDateTime = DateTime.UtcNow;
            }
            
            //сохраняем все внесенные изменения
            await _dbContext.SaveChangesAsync();
            //конец транзакции БД
            await EfTransaction.CommitAsync();

            return transaction;
        }

        public async Task<DebitTransaction> AddDebitTransaction(DebitTransaction transaction)
        {
            //Начинаем транзакцию БД
            await using var EfTransaction = await _dbContext.Database.BeginTransactionAsync();

            //Получаем баланс клиента
            var clientBalance = await _dbContext.ClientBalances.FirstOrDefaultAsync(x => x.ClientId == transaction.ClientId);

            if (clientBalance == null)
            {
                throw new NotFoundException("Невозможно определить баланс клиента для списания, так как клиента нет в базе. Выгрузите либо транзакцию пополнения баланса, либо информацию о балансе клиента");
            }

            decimal currentBalance = clientBalance.Balance;//clientBalance?.Balance ?? 0m;

            //вычисляем баланс клиента с учетом транзакции
            var newBalance = transaction.CalculateBalance(currentBalance);
            //баланс не может быть меньше 0
            if (newBalance < 0m)
            {
                throw new Exception("На балансе клиента недостаточно средств для проведения операции");
            }

            transaction.ClientBalanceAfterAddTransaction = newBalance;
            transaction.InsertDateTime = DateTime.UtcNow;

            //обновляем баланс клиента
            clientBalance.Balance = newBalance;
            clientBalance.LastCalculationDateTime = DateTime.UtcNow;
            //clientBalance.ApplyBalance(newBalance);

            //добавляем транзакцию в базу
            await _dbContext.Transactions.AddAsync(transaction);

            //сохраняем все внесенные изменения
            await _dbContext.SaveChangesAsync();
            //конец транзакции БД
            await EfTransaction.CommitAsync();

            return transaction;
        }

        public async Task<Transaction> RevertTransaction(Transaction transaction)
        {
            //Начинаем транзакцию БД
            await using var EfTransaction = await _dbContext.Database.BeginTransactionAsync();

            var clientBalance = await _dbContext.ClientBalances.FirstOrDefaultAsync(x => x.ClientId == transaction.ClientId);

            if (clientBalance == null)
            {
                throw new NotFoundException("Клиент не найден в базе данных. нет данных о балансе клиента для проведения операции");
            }
            
            transaction.IsReverted = true;
            transaction.RevertDateTime = DateTime.UtcNow;

            //вычисляем баланс клиента с учетом транзакции
            if (transaction is not ICalculateBalance transactionCalcBalance)
            {
                throw new Exception("Невозможно пересчитать баланс для данной транзакции");
            }

            decimal currentBalance = clientBalance.Balance;
            
            var newBalance = transactionCalcBalance.CalculateBalance(currentBalance);
            //баланс не может быть меньше 0
            if (newBalance < 0m)
            {
                throw new Exception("На балансе клиента недостаточно средств для проведения операции");
            }

            transaction.ClientBalanceAfterRevertTransaction = newBalance;

            clientBalance.Balance = newBalance;
            clientBalance.LastCalculationDateTime = DateTime.UtcNow;
            
            //сохраняем изменения в базу
            await _dbContext.SaveChangesAsync();
            //Завершаем транзакцию
            await EfTransaction.CommitAsync();

            return transaction;
        }

        public async Task<ClientBalance?> GetClientBalanceById(Guid Id)
        {
            var balance = await _dbContext.ClientBalances.FirstOrDefaultAsync(x => x.ClientId == Id);

            return balance;
        }

        public async Task<Transaction?> GetTransactionById(Guid Id)
        {
            var transactionInDb = await _dbContext.Transactions.FirstOrDefaultAsync(x => x.Id == Id);
            
            return transactionInDb;
        }

        
    }
}
