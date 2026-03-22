using FinanceFlow.Application.Common.DTOs;
using FinanceFlow.Application.Common.Interfaces;
using FinanceFlow.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinanceFlow.Infrastructure.Identity
{
    public class JWTGenerator : IJWTGenerator
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;

        public JWTGenerator(IConfiguration configuration
            ,UserManager<ApplicationUser> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
        }
        public async Task<LoginResponse> CreateTokenAsync(User user)
        {

            var AuthClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };
            var AppUser=await userManager.FindByIdAsync(user.Id);
            if (AppUser is not null)
            {
                var userRoles = await userManager.GetRolesAsync(AppUser);
                foreach (var role in userRoles)
                {
                    AuthClaim.Add(new Claim(ClaimTypes.Role, role));
                }

            }
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            var Token = new JwtSecurityToken(
                 issuer: configuration["JWT:ValidIssuer"],
                 audience: configuration["JWT:ValidAudience"],
                 expires: DateTime.Now.AddDays(double.Parse(configuration["JWT:DuratoinInDays"])),
                 claims: AuthClaim,
                 signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );
            var token = new JwtSecurityTokenHandler().WriteToken(Token);
            var res = new LoginResponse(token, true);
            Console.WriteLine(res.Token);
            return res;

        }
    }
}
