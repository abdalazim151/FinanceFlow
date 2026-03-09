using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceFlow.Application.Common.DTOs;
using FinanceFlow.Application.Common.Interfaces;
using FinanceFlow.Domain.Entities;
using FinanceFlow.Domain.Enums;
using FinanceFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceFlow.Infrastructure
{
    public class AtmService : IAtmService
    {
        private readonly ApplicationDbContext context;

        public AtmService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> BankFeedAsync(int atmId, int amount, string description)
        {
            if (amount <= 0 || amount % 10 != 0)
            {
                return false;
            }

            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var atm = await context.AtmMachines
                    .Include(a => a.Inventories)
                    .FirstOrDefaultAsync(a => a.Id == atmId);

                if (atm is null)
                {
                    return false;
                }

                var denominations = new[] { 200, 100, 50, 20, 10 };
                var remaining = amount;

                foreach (var denom in denominations)
                {
                    var count = remaining / denom;
                    if (count <= 0) continue;

                    var inventory = atm.Inventories.FirstOrDefault(i => (int)i.Denomination == denom);
                    if (inventory != null)
                    {
                        inventory.Count += count;
                    }

                    remaining -= count * denom;
                }

                var transactionEntity = new Transaction
                {
                    Amount = amount,
                    transactionType = TransactionType.Feed,
                    User1Id = string.Empty,
                    User2Id = string.Empty,
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

        public async Task<decimal> GetUserBalanceAsync(string accountId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == accountId);
            return user?.Balance ?? 0;
        }

        public async Task<int> GetAtmBalanceAsync(int atmId)
        {
            var atm = await context.AtmMachines
                .Include(a => a.Inventories)
                .FirstOrDefaultAsync(a => a.Id == atmId);

            if (atm is null)
            {
                return 0;
            }

            return atm.Inventories.Sum(inv => (int)inv.Denomination * inv.Count);
        }

        public async Task<IReadOnlyList<AtmDto>> GetAllAtmsAsync()
        {
            var atms = await context.AtmMachines
                .Include(a => a.Inventories)
                .ToListAsync();

            return atms
                .Select(a => new AtmDto
                {
                    Id = a.Id,
                    Location = a.Location,
                    TotalCash = a.Inventories.Sum(inv => (int)inv.Denomination * inv.Count)
                })
                .ToList();
        }

        public async Task<AtmDto> CreateAtmAsync(string location)
        {
            var atm = new AtmMachine
            {
                Location = location
            };

            context.AtmMachines.Add(atm);
            await context.SaveChangesAsync();

            return new AtmDto
            {
                Id = atm.Id,
                Location = atm.Location,
                TotalCash = 0
            };
        }
    }
}

