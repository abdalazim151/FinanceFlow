using FinanceFlow.Domain.MessagingContract;
using FinanceFlow.Infrastructure.Identity;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;

namespace FinanceFlow.Infrastructure.Serivces
{
    public  class ReportService
    {
        private readonly IMongoDatabase mongo;
        private readonly IConfiguration configuration;
        private readonly IPublishEndpoint publish;
        private readonly UserManager<ApplicationUser> context;

        public ReportService(IMongoDatabase mongo
            , IConfiguration configuration
            , IPublishEndpoint publish
            , UserManager<ApplicationUser> context)
        {
            this.mongo = mongo;
            this.configuration = configuration;
            this.publish = publish;
            this.context = context;
        }
        public async Task Generate(string Type,string email)
        {
            var user=await context.FindByEmailAsync(email);
            if (user is null) {
                return;
            }
            var collection = mongo.GetCollection<BsonDocument>("TransActions");
            StringBuilder content =new StringBuilder("");
            string title = $"{Type} Report";
            var data =new List<BsonDocument>();
            switch (Type)
            {
                case "Feed":
                case "Deposit":
                case "Withdraw":
                case "Transfer":
                     data = collection.AsQueryable()
                    .Where(u => u["TransactionType"] == Type)
                     .OrderByDescending(t => t["CreatedAt"])
                     .Take(20)
                     .ToList();
                    break;
                default:
                    data = collection.AsQueryable()
                    .OrderByDescending(t => t["CreatedAt"])
                    .Take(20)
                    .ToList();
                    title = "General Report";
                    break;
            }
            for (int i = 0; i < data.Count; i++)
            {
                string msg = await GenerateMsg(data[i]);
                content.AppendLine(msg);
            }

            var notiContract = new NotificationContract()
            {
                Content = content.ToString(),
                Email = email,
                Subject = title
            };
            publish.Publish(notiContract);

        }
        private async Task<string> GenerateMsg(BsonDocument data)
        {
            if (data["TransactionType"] == "Withdraw" ||
                            data["TransactionType"] == "Deposit")
                return $"{data["TransactionType"]} Amount {data["Amount"]} to ATM at location: {data["AtmLocation"]}    {data["CreatedAt"]}";
            else if (data["TransactionType"] == "Feed")
                return $"Feed Amount {data["Amount"]} to ATM at location: {data["AtmLocation"]}       {data["CreatedAt"]}";
            else if (data["TransactionType"] == "Transfer")
                return $"Transfer Amount {data["Amount"]} To User {data["To"]}      {data["CreatedAt"]}";
            else return "null transaction";
        }
    }
}
