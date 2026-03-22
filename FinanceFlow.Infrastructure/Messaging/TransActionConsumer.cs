using FinanceFlow.Domain.Entities;
using FinanceFlow.Domain.Enums;
using FinanceFlow.Domain.MessagingContract;
using FinanceFlow.Infrastructure.Serivces;
using MassTransit;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Infrastructure.Messaging
{
    public class TransActionConsumer : IConsumer<TransActionContract>
    {
        private readonly IPublishEndpoint endpoint;
        private readonly TransActionService service;

        public TransActionConsumer(IPublishEndpoint endpoint,TransActionService service)
        {
            this.endpoint = endpoint;
            this.service = service;
        }
        public async Task Consume(ConsumeContext<TransActionContract> context)
        {
            var transactionData = context.Message;
            await service.SaveTransAction(transactionData);
        
            Console.WriteLine($"Transaction {transactionData.Id} processed successfully!");
        }
    }
}
