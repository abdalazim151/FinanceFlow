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
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.transactionType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
           
            builder.HasOne(t => t.atmMachine).WithMany(a => a.transactions)
                .HasForeignKey(a =>a.AtmMachineId)
                .OnDelete(DeleteBehavior.NoAction);
        
        }   
    }
}
