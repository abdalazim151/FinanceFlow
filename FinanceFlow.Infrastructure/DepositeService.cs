using FinanceFlow.Application.Common.Interfaces;
using FinanceFlow.Domain.Entities;
using FinanceFlow.Domain.Enums;
using FinanceFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Infrastructure
{
    public class DepositeService : IDepositeService
    {
        private readonly ApplicationDbContext context;

        public DepositeService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<bool> Deposite(int amount, string AccountId, int AtmId, string disc = "string disc")
        {
            //--begin transaction
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                if (amount % 10 != 0) return false;
                var res = await Calc(amount);
                var user = context.Users.FirstOrDefault(u => u.Id == AccountId);
                if (user == null)
                    return false;
                var atm = context.AtmMachines
                    .Include(i => i.Inventories)
                    .FirstOrDefault(a => a.Id == AtmId);
                if (atm == null) return false;
                atm.Inventories.ToList().ForEach(i =>
                {
                    if (res.ContainsKey((int)i.Denomination))
                    {
                        i.Count += res[(int)i.Denomination];
                    }
                });
                user.Balance += amount;

                var transactionEntity = new Transaction
                {
                    Amount = amount,
                    transactionType = TransactionType.Deposit,
                    User1Id = user.Id,
                    User2Id = string.Empty,
                    AtmMachineId = atm.Id,
                    atmMachine = atm
                };

                context.Transactions.Add(transactionEntity);
                context.SaveChanges();
                await transaction.CommitAsync(); // commiting transaction
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await transaction.RollbackAsync(); // rolling back transaction in case of concurrency exception
                return false;
            }
            return true;
        }

        public async Task<Dictionary<int, int>> Calc(int amount)
        {
            var result = new Dictionary<int, int>();
            var list = new List<int>();
            list.Add(200);
            list.Add(100);
            list.Add(50);
            list.Add(20);
            list.Add(10);
            for (int i = 0; i < list.Count; i++)
            {
                var count = amount / list[i];
                result.Add(list[i], count);
                amount -= count * list[i];
            }

            return result;
        }
    }
}
