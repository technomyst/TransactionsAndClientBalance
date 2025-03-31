using Unistream_T4.Models.Abstractions;

namespace Unistream_T4.Models.Transactions
{
    public class CreditTransaction : Transaction,ICalculateBalance
    {
        public decimal CalculateBalance(decimal balance)
        {
            return IsReverted ? balance - Amount : balance + Amount;
        }

    }
}
