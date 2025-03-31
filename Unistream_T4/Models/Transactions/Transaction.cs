using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Unistream_T4.Models.Abstractions;
using Unistream_T4.Validators;

namespace Unistream_T4.Models.Transactions
{
    public class Transaction : ITransaction
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        public TimeSpan TimeZoneOffset { get; set; }
        public DateTime InsertDateTime { get; set; }
        public decimal? ClientBalanceAfterAddTransaction { get; set; }
        public bool IsReverted { get; set; } = false;
        public DateTime? RevertDateTime { get; set; }
        public decimal? ClientBalanceAfterRevertTransaction { get; set; }
        
    }
}
