using FinanceFlow.Application.Common.Interfaces;
using FinanceFlow.Domain.Entities;
using FinanceFlow.Domain.Enums;
using FinanceFlow.Domain.MessagingContract;
using FinanceFlow.Infrastructure.Persistence;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Infrastructure.Serivces
{
    public class WithdrawService : IWithdrawService
    {
        private readonly ApplicationDbContext context;
        private readonly IPublishEndpoint publish;

        public WithdrawService(ApplicationDbContext context,IPublishEndpoint publish)
        { 
            this.context = context;
            this.publish = publish;
        }
        public async Task<bool> CheckValidAmount(int amount, string AccountId, int AtmId)
        {

            var atm = await context.AtmMachines
                .Include(i => i.Inventories)
                .FirstOrDefaultAsync(u => u.Id == AtmId);

            if (atm is null) return false;

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == AccountId);
            if (user is null || user.Balance < amount) return false;

            int temp = amount;

            var sortedInventories = atm.Inventories.OrderByDescending(i => i.Denomination).ToList();

            foreach (var item in sortedInventories)
            {
                if (temp == 0) break;

                int billValue = (int)item.Denomination;
                int needed = temp / billValue;

                int actualToTake = Math.Min(needed, item.Count);
                temp -= actualToTake * billValue;
            }
            return temp == 0;
        }

        public async Task<bool> Withdraw(int amount, string AccountId, int AtmId, string disc = "Living expenses")
        {
            if (amount <= 0 || amount % 10 != 0) return false;

            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == AccountId);
                var atm = await context.AtmMachines
                    .Include(i => i.Inventories)
                    .FirstOrDefaultAsync(a => a.Id == AtmId);

                if (user == null || atm == null || user.Balance < amount)
                    return false;

                int remainingToWithdraw = amount;

                var sortedInventories = atm.Inventories
                    .OrderByDescending(i => i.Denomination)
                    .ToList();

                foreach (var inventory in sortedInventories)
                {
                    if (remainingToWithdraw <= 0) break;

                    int billValue = (int)inventory.Denomination;
                    int neededCount = remainingToWithdraw / billValue;

                    // إذا كنا نحتاج هذه الفئة وهي متوفرة في الماكينة
                    if (neededCount > 0 && inventory.Count > 0)
                    {
                        int actualTaken = Math.Min(neededCount, inventory.Count);

                        // خصم من مخزون الماكينة
                        inventory.Count -= actualTaken;
                        remainingToWithdraw -= actualTaken * billValue;
                    }
                }

                if (remainingToWithdraw > 0)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
                user.Balance -= amount;

                var transactionEntity = new TransActionContract
                {
                    Amount = amount,
                    transactionType = TransactionType.Withdraw,
                    User1Id = user.Id,
                    User2Id = null,
                    AtmMachineId = atm.Id,
                };
                await context.SaveChangesAsync();
                publish.Publish(transactionEntity);
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
                if (transaction != null) await transaction.RollbackAsync();
                return false;
            }
        }
    }
}
