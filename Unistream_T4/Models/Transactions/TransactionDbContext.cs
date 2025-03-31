using Microsoft.EntityFrameworkCore;
using Unistream_T4.Models.Abstractions;

namespace Unistream_T4.Models.Transactions
{
    public class TransactionDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<CreditTransaction> CreditTransactions { get; set; } = null!;
        public DbSet<DebitTransaction> DebitTransactions { get; set; } = null!;
        public DbSet<ClientBalance> ClientBalances { get; set; } = null!;

        public TransactionDbContext(DbContextOptions<TransactionDbContext> opts) : base(opts)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
