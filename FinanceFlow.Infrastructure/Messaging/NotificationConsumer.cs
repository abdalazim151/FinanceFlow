using FinanceFlow.Domain.MessagingContract;
using FinanceFlow.Infrastructure.Serivces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Infrastructure.Messaging
{
    public class NotificationConsumer:IConsumer<NotificationContract>
    {
        private readonly EmailService emailService;

        public NotificationConsumer(EmailService emailService)
        {
            this.emailService = emailService;
        }

        public Task Consume(ConsumeContext<NotificationContract> context)
        {
            var message = context.Message;
            emailService.Send(message.Email, message.Subject, message.Content);
            Console.WriteLine($"Notification sent to {message.Email} with subject: {message.Subject}");
            return Task.CompletedTask;
        }
    }
}
