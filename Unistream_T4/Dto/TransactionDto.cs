using Unistream_T4.Validators;

namespace Unistream_T4.Dto
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        [NotFutureDate(ErrorMessage = "Дата транзакции должна быть равна или меньше текущей")]
        public DateTimeOffset DateTime { get; set; }
        [IsPositiveDecimal(ErrorMessage = "Сумма транзакции должна быть положительной")]
        public decimal Amount { get; set; }

    }
}
