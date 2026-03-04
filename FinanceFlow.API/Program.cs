
using FinanceFlow.Application;
using FinanceFlow.Application.Common.Interfaces;
using FinanceFlow.Application.Features.Authentication.Commands;
using FinanceFlow.Infrastructure;
using FinanceFlow.Infrastructure.Identity;
using FinanceFlow.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceFlow.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    var connection = builder.Configuration.GetConnectionString("Local");
                    options.UseSqlServer(connection);
                });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<string>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJWTGenerator, JWTGenerator>();
            builder.Services.AddScoped<IDepositeService, DepositeService>();
            builder.Services.AddScoped<IWithdrawService, WithdrawService>();
            builder.Services.AddScoped<ITransferService, TransferService>();
            builder.Services.AddScoped<IAtmService, AtmService>();
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly((typeof(LoginCommand).Assembly));
            });
            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var loggerfactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var Dbcontext = services.GetRequiredService<ApplicationDbContext>();
                await Dbcontext.Database.MigrateAsync();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            }
            catch (Exception ex)
            {
                var logger = loggerfactory.CreateLogger<Program>();
                logger.LogError(ex, "an error occured during applying db");
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
