using FinanceFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceFlow.Infrastructure.Persistence.Configurations
{
    public class AtmInventoryConfiguration : IEntityTypeConfiguration<AtmInventory>
    {
        public void Configure(EntityTypeBuilder<AtmInventory> builder)
        {
            builder.HasKey(ai => ai.Id);

            builder.HasOne(ai => ai.AtmMachine)
                .WithMany(a => a.Inventories)
                .HasForeignKey(ai => ai.AtmMachineId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

