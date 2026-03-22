using FinanceFlow.Application.Common.Behaviours;
using FinanceFlow.Application.Common.Interfaces;
using FinanceFlow.Application.Features.Authentication.Commands;
using FinanceFlow.Infrastructure.Identity;
using FinanceFlow.Infrastructure.Messaging;
using FinanceFlow.Infrastructure.Serivces; // تأكد من السبيلنج ده عندك
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceFlow.Infrastructure
{
    public static class AddServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
            services.AddMassTransit(x =>
            {
                x.AddConsumer<TransActionConsumer>();
                x.AddConsumer<NotificationConsumer>();
                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ReceiveEndpoint("transaction-queue", e =>
                    {
                        e.ConfigureConsumer<TransActionConsumer>(context);
                    });
                    cfg.ReceiveEndpoint("Notification-queue", e =>
                    {
                        e.ConfigureConsumer<NotificationConsumer>(context);
                    });

                });
            });
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJWTGenerator, JWTGenerator>();
            services.AddScoped<IDepositeService, DepositeService>();
            services.AddScoped<IWithdrawService, WithdrawService>();
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<IAtmService, AtmService>();
            services.AddScoped<TransActionService>();

            services.AddScoped<EmailService>();
            services.AddScoped<ReportService>();

            return services;
        }
    }
}