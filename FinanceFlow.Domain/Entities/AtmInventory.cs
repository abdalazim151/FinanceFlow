using FinanceFlow.Domain.Enums;

namespace FinanceFlow.Domain.Entities
{
    public class AtmInventory
    {
        public int Id { get; set; }

        public Denomination Denomination { get; set; }
        public int Count { get; set; }
        public int AtmMachineId { get; set; }
        public AtmMachine AtmMachine { get; set; } = null!;
    }
}
