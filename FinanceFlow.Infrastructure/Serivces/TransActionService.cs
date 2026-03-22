using FinanceFlow.Application.Features.Operation.Command;
using FinanceFlow.Domain.Entities;
using FinanceFlow.Domain.Enums;
using FinanceFlow.Domain.MessagingContract;
using FinanceFlow.Infrastructure.Identity;
using FinanceFlow.Infrastructure.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Infrastructure.Serivces
{
    public class TransActionService
    {
        private readonly IMongoDatabase database;
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPublishEndpoint publish;

        public TransActionService(IMongoDatabase database
            ,ApplicationDbContext dbContext
            ,UserManager<ApplicationUser> userManager
            ,IPublishEndpoint publish)
        {
            this.database = database;
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.publish = publish;
        }
        public async Task<bool> SaveTransAction(TransActionContract transaction)
        {
            var collection = database.GetCollection<BsonDocument>("TransActions");
            var atmMachine= transaction.AtmMachineId is null? null: await dbContext.AtmMachines.FindAsync(transaction.AtmMachineId);
            
            switch (transaction.transactionType)
            {
                case TransactionType.Deposit:

                    var DepositDocument = new BsonDocument
                        {
                            { "Amount", transaction.Amount },
                            { "TransactionType", transaction.transactionType.ToString() },
                            { "CreatedAt", transaction.CreatedAt },
                            { "AtmLocation", atmMachine?.Location },
                            { "From",transaction.User1Id} 
                        };
                    await collection.InsertOneAsync(DepositDocument);
                    var user =await  userManager.FindByIdAsync(transaction.User1Id);
                    var msg = $"You make Deposite from Atm whith location {atmMachine?.Location} at {transaction.CreatedAt},   Amout : {transaction.Amount}";
                    var msgContract = new NotificationContract()
                    {
                        Content = msg,
                        Email = user.Email,
                        Subject = "Deposite Notification"
                    };
                    publish.Publish(msgContract);
                    break;
                case TransactionType.Withdraw:
                    var WithdrawalDocument = new BsonDocument
                        {
                            { "Amount", transaction.Amount },
                            { "TransactionType", transaction.transactionType.ToString() },
                            { "CreatedAt", transaction.CreatedAt },
                            { "AtmLocation", atmMachine?.Location },
                            {"From",transaction.User1Id }
                        };
                    await collection.InsertOneAsync(WithdrawalDocument);
                    var user2 = await userManager.FindByIdAsync(transaction.User1Id);
                    var msg2 = $"You make Withdraw from Atm whith location {atmMachine?.Location} at {transaction.CreatedAt},   Amout : {transaction.Amount}";
                    var msgContract2 = new NotificationContract()
                    {
                        Content = msg2,
                        Email = user2.Email,
                        Subject = "Withdraw Notification"
                    };
                    publish.Publish(msgContract2);
                    break;
                case TransactionType.Transfer:
                    var TransferDocument = new BsonDocument
                        {
                            { "Amount", transaction.Amount },
                            { "TransactionType", transaction.transactionType.ToString() },
                            { "CreatedAt", transaction.CreatedAt },
                            { "From",transaction.User1Id },
                            { "To", transaction.User2Id }
                        
                        };
                    await collection.InsertOneAsync(TransferDocument);
                    var user3 = await userManager.FindByIdAsync(transaction.User1Id);
                    var user4 = await userManager.FindByIdAsync(transaction.User2Id);
                   var msg3 = $"You make Transfer to {user4.Email} at {transaction.CreatedAt},   Amout : {transaction.Amount}";
                    var msg4 =$"You have Received Transfer from {user3.Email} at {transaction.CreatedAt},   Amout : {transaction.Amount}";
                     var msgContract3 = new NotificationContract()
                     {
                        Content = msg3,
                        Email = user3.Email,
                        Subject = "Transfer Notification"
                    };

                    var msgContract4 = new NotificationContract()
                    {
                        Email = user4.Email,
                        Content = msg4,
                        Subject = "Transfer Notification"

                    };
                    publish.Publish(msgContract3);
                    publish.Publish(msgContract4);
                    break;
                case TransactionType.Feed:
                    var FeedDocument = new BsonDocument
                        {
                            { "Amount", transaction.Amount },
                            { "TransactionType", transaction.transactionType.ToString() },
                            { "CreatedAt", transaction.CreatedAt },
                            { "AtmLocation", atmMachine?.Location },
                        };
                    await collection.InsertOneAsync(FeedDocument);
                    break;
            }
            return true;
        }
    }
}
