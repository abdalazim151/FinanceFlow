using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Domain.Entities
{
    public class AtmMachine
    {
        public int Id { get; set; }
        public string Location { get; set; } = string.Empty;
        public ICollection<AtmInventory> Inventories { get; set; } = new List<AtmInventory>();

        public decimal TotalCash => Inventories.Sum(inv => (int)inv.Denomination * inv.Count);
    }
}
