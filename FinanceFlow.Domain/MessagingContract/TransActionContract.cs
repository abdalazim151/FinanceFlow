using FinanceFlow.Domain.Entities;
using FinanceFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Domain.MessagingContract
{
    public class TransActionContract
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public TransactionType transactionType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? User1Id { get; set; } = string.Empty;
        public string? User2Id { get; set; } = string.Empty;
        public int? AtmMachineId { get; set; }
    }
}
