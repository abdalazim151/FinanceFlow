
using FinanceFlow.API.Middlewares;
using FinanceFlow.Application.Features.Authentication.Commands;
using FinanceFlow.Infrastructure;
using FinanceFlow.Infrastructure.Identity;
using FinanceFlow.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using System.Threading.RateLimiting;

namespace FinanceFlow.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers()
                .AddFluentValidation();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #region dataBase
            try
            {
                builder.Services.AddDbContext<ApplicationDbContext>(
                    options =>
                    {
                        var connection = builder.Configuration.GetConnectionString("Local");
                        options.UseSqlServer(connection);
                    });
                builder.Services.AddIdentity<ApplicationUser, IdentityRole>() // شلنا الـ <string> لأن IdentityRole هي أصلاً string باي ديفولت
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders();
                var mongoSettings = builder.Configuration.GetSection("MongoDB");

                builder.Services.AddSingleton<IMongoClient>(sp =>
                {
                    return new MongoClient(mongoSettings["ConnectionString"]);
                });

                builder.Services.AddScoped(sp =>
                {
                    var client = sp.GetRequiredService<IMongoClient>();
                    return client.GetDatabase(mongoSettings["DatabaseName"]);
                });
            }catch(Exception ex)
            {
                   Console.WriteLine(ex.Message);
            }
            
            #endregion
            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                // 2. استخدام التوزيع (Partitioning) بناءً على الـ IP أو المستخدم
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    // تمييز كل مستخدم عن التاني (بناءً على الـ IP أو الـ Identity لو مسجل دخول)
                    var key = httpContext.User.Identity?.Name
                              ?? httpContext.Connection.RemoteIpAddress?.ToString()
                              ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5 ,                   // 5 طلبات
                        Window = TimeSpan.FromSeconds(10),    // كل 10 ثواني
                        QueueLimit = 5,                       // طابور بحد أقصى 5 
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });
                });
            });

            builder.Services.AddInfrastructureServices();
            var jwtSettings = builder.Configuration.GetSection("JWT");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["ValidIssuer"],
                    ValidAudience = jwtSettings["ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
            object value = builder.Services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();

            var app = builder.Build();
            using var ScopedServices = app.Services.CreateScope();// create instance of scoped services

            var Services = ScopedServices.ServiceProvider; // get all scoped services

            var roleManager = Services.GetRequiredService<RoleManager<IdentityRole>>();
                await RoleSeeding.SeedRolesAsync(roleManager);
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
            #region Middleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();
            app.MapControllers();
            #endregion
            app.Run();
        }
    }
}
/*
 * message queue
one for saving transaction,
one for sending notifications,
one for generating reports,
 */
