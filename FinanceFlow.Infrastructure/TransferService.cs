using System;
using System.Threading.Tasks;
using FinanceFlow.Application.Common.Interfaces;
using FinanceFlow.Domain.Entities;
using FinanceFlow.Domain.Enums;
using FinanceFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceFlow.Infrastructure
{
    public class TransferService : ITransferService
    {
        private readonly ApplicationDbContext context;

        public TransferService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> TransferAsync(string fromAccountId, string toAccountId, int amount, int atmId, string description)
        {
            if (amount <= 0)
            {
                return false;
            }

            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var fromUser = await context.Users.FirstOrDefaultAsync(u => u.Id == fromAccountId);
                var toUser = await context.Users.FirstOrDefaultAsync(u => u.Id == toAccountId);
                var atm = await context.AtmMachines.FirstOrDefaultAsync(a => a.Id == atmId);

                if (fromUser is null || toUser is null || atm is null)
                {
                    return false;
                }

                if (fromUser.Balance < amount)
                {
                    return false;
                }

                fromUser.Balance -= amount;
                toUser.Balance += amount;

                var transactionEntity = new Transaction
                {
                    Amount = amount,
                    transactionType = TransactionType.Transfer,
                    User1Id = fromUser.Id,
                    User2Id = toUser.Id,
                    AtmMachineId = atm.Id,
                    atmMachine = atm
                };

                context.Transactions.Add(transactionEntity);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                return false;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}

