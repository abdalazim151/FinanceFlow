using FinanceFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public TransactionType transactionType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;
        public int AtmMachineId { get; set; }
        public AtmMachine? atmMachine { get; set; }
    }
}
