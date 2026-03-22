using System;
using System.Threading.Tasks;
using FinanceFlow.Application.Common.Interfaces;
using FinanceFlow.Domain.Entities;
using FinanceFlow.Domain.Enums;
using FinanceFlow.Domain.MessagingContract;
using FinanceFlow.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace FinanceFlow.Infrastructure.Serivces
{
    public class TransferService : ITransferService
    {
        private readonly ApplicationDbContext context;
        private readonly IPublishEndpoint publish;

        public TransferService(ApplicationDbContext context
            ,IPublishEndpoint publish)
        {
            this.context = context;
            this.publish = publish;
        }

        public async Task<bool> TransferAsync(string fromAccountId, string toAccountId, int amount, string description)
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

                if (fromUser is null || toUser is null )
                {
                    return false;
                }

                if (fromUser.Balance < amount)
                {
                    return false;
                }

                fromUser.Balance -= amount;
                toUser.Balance += amount;

                var transactionEntity = new TransActionContract
                {
                    Amount = amount,
                    transactionType = TransactionType.Transfer,
                    User1Id = fromUser.Id,
                    User2Id = toUser.Id,
                    CreatedAt = DateTime.UtcNow,
                };

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                publish.Publish(transactionEntity);

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

