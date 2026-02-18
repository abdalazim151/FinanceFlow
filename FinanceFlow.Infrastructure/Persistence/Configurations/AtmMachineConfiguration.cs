using FinanceFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Infrastructure.Persistence.Configurations
{
    public class AtmMachineConfiguration : IEntityTypeConfiguration<AtmMachine>
    {
        void IEntityTypeConfiguration<AtmMachine>.Configure(EntityTypeBuilder<AtmMachine> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Location).HasMaxLength(200);
            
        }
    }
}
