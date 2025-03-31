namespace Unistream_T4.Models.Abstractions
{
    public interface ITransaction
    {
        Guid Id { get; }
        Guid ClientId { get; }
        DateTime DateTime { get; }
        decimal Amount { get; }
    }
}
