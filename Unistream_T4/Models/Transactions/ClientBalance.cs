using System.ComponentModel.DataAnnotations;
using Unistream_T4.Models.Abstractions;

namespace Unistream_T4.Models.Transactions
{
    public class ClientBalance
    {
        [Key]
        public Guid ClientId { get; set; }
        public decimal Balance { get; set; }
        public DateTime LastCalculationDateTime { get; set; }
        


        //public decimal ApplyTransaction(ICalculateBalance transaction)
        //{
        //    var balanceAfterApplyingTransaction = transaction.CalculateBalance(Balance);

        //    if (balanceAfterApplyingTransaction < 0m)
        //    {
        //        throw new Exception("На балансе клиента недостаточно средств для проведения операции");
        //    }

        //    Balance = balanceAfterApplyingTransaction;
        //    LastCalculationDateTime= DateTime.UtcNow;

        //    return balanceAfterApplyingTransaction;
        //}

    }
}
