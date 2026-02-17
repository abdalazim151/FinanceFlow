using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Domain.Entities
{
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public byte[] RowVersion { get; set; } = null!;
    }
}
