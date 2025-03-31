using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using Unistream_T4.Dto;
using Unistream_T4.Exceptions;
using Unistream_T4.Models.Abstractions;
using Unistream_T4.Models.Transactions;
using Unistream_T4.Repositories.Abstractions;
using Unistream_T4.Services.Abstractions;

namespace Unistream_T4.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _repository;
        public TransactionService( IMapper mapper, ITransactionRepository repository)
        {
            _mapper=mapper;
            _repository=repository;
        }

        public async Task<AddTransactionResponseDto> AddDebitTransaction(TransactionDto transactionDto)
        {
            //Методы POST должны быть идемпотентными: при повторной отправке запроса он должен
            //возвращать результат отправки предыдущего запроса с таким же Id.

            var transactionInDb=await _repository.GetTransactionById(transactionDto.Id);
            if (transactionInDb != null)
            {
                var responseFromDb = _mapper.Map<AddTransactionResponseDto>(transactionInDb);
                return responseFromDb;
            }

            var transaction = _mapper.Map<DebitTransaction>(transactionDto);

            if (transaction == null)
            {
                throw new ArgumentException("Ошибка маппинга");
            }

            //записываем транзакцию и пересчитываем баланс в транзакции БД
            await _repository.AddDebitTransaction(transaction);


            var response = _mapper.Map<AddTransactionResponseDto>(transaction);
            return response;
        }

        public async Task<AddTransactionResponseDto> AddCreditTransaction(TransactionDto transactionDto)
        {
            //Методы POST должны быть идемпотентными: при повторной отправке запроса он должен
            //возвращать результат отправки предыдущего запроса с таким же Id.

            var transactionInDb = await _repository.GetTransactionById(transactionDto.Id);
            if (transactionInDb != null)
            {
                var responseFromDb = _mapper.Map<AddTransactionResponseDto>(transactionInDb);
                return responseFromDb;
            }

            var transaction = _mapper.Map<CreditTransaction>(transactionDto);

            if (transaction == null)
            {
                throw new ArgumentException("Ошибка маппинга");
            }

            //записываем транзакцию и пересчитываем баланс в транзакции БД
            await _repository.AddCreditTransaction(transaction);


            var response = _mapper.Map<AddTransactionResponseDto>(transaction);
            return response;

        }

        public async Task<RevertTransactionResponseDto> RevertTransaction(Guid id)
        {
            var transactionInDb = await _repository.GetTransactionById(id);
            
            if (transactionInDb == null)
            {
                throw new NotFoundException("Транзакция не найдена в базе данных");
            }

            if (transactionInDb.IsReverted)//транзакция уже отменена. Соблюдаем идемпотентность.
            { 
                var responseFromDb = _mapper.Map<RevertTransactionResponseDto>(transactionInDb);
                return responseFromDb;
            }
            //записываем транзакцию и пересчитываем баланс в транзакции БД
            await _repository.RevertTransaction(transactionInDb);

            var response = _mapper.Map<RevertTransactionResponseDto>(transactionInDb);
            return response;
        }

        public async Task<ClientBalanceReponseDto> GetClientBalance(Guid id)
        {
            var clientBalance = await _repository.GetClientBalanceById(id);
            
            if (clientBalance == null)
            {
                throw new NotFoundException("Клиент с указанным id не найден");
            }

            var response = _mapper.Map<ClientBalanceReponseDto>(clientBalance);
            return response;
        }

       
        //public async Task<bool> UpdateClientBalance(Guid id, Transaction transaction)
        //{
        //    var clientBalance = await _dbContext.ClientBalances.FirstOrDefaultAsync(x => x.ClientId == transaction.ClientId);

        //    clientBalance.Balance = clientBalance.Balance + transaction.Amount;
        //    clientBalance.LastCalculationDateTime = DateTime.UtcNow;

        //    var resultOnSave = await _dbContext.SaveChangesAsync();
        //}
    }
}
