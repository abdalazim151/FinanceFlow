using FinanceFlow.Application.Common.DTOs;
using FinanceFlow.Application.Common.Interfaces;
using FinanceFlow.Application.Features.Authentication.Commands;
using FinanceFlow.Domain.Entities;
using FinanceFlow.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceFlow.Infrastructure.Identity
{

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IJWTGenerator jWTGenerator;

        public AuthService(ApplicationDbContext _dbContext
            , UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
            , IJWTGenerator jWTGenerator
           
            )
        {
            this.dbContext = _dbContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jWTGenerator = jWTGenerator;
        }
        public async Task<LoginResponse> LoginAsync(LoginCommand command)
        {
            var user = await userManager.FindByEmailAsync(command.Email);
            var acUser=await dbContext.Users.FirstOrDefaultAsync(u=>u.Id==user.Id);
            if (user is null || acUser is null)
                return new LoginResponse("No Such User", false);
               
            var result = await signInManager.CheckPasswordSignInAsync(user, command.password, false);
            if (!result.Succeeded) return new LoginResponse("", false);
            var Res = await jWTGenerator.CreateTokenAsync(acUser);
            Console.WriteLine(Res.Token);
            Console.WriteLine(Res.IsSuccess);
            return Res;
        }
        public async Task<RegisterResponse> RegisterAsync(RegisterCommand command)
        {
            var user = await userManager.FindByEmailAsync(command.Email);
            if (user !=null )
                return new RegisterResponse("User with this email already exists", false);
            if (command.Password != command.ConfirmPassword)
                return new RegisterResponse("Password and Confirm Password do not match", false);
            if (command.Password.Length < 6 || command.Password.Length > 20)
                return new RegisterResponse("Password must be between 6 and 20", false);
            var newUser = new ApplicationUser()
            {
                Email = command.Email,
                UserName = command.Email
            };
            var res = await userManager.CreateAsync(newUser, command.Password);
            if (!res.Succeeded)
                return new RegisterResponse("Error While Register User", false);
            user=userManager.Users.FirstOrDefault(u => u.Email == command.Email);
            var NewDUser = new User()
            {
                FullName = command.FullName,
                Balance = 0,
                Id = user.Id
         
            };
            try
            {
                dbContext.Users.Add(NewDUser);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return new RegisterResponse("Error While Register User", false);
            }
            return new RegisterResponse("User Registered Successfully", true);   

        }
    }
}
