using AuthenticationAPI.Application.DTO;
using AuthenticationAPI.Application.Interface;
using AuthenticationAPI.Domain.Entities;
using AuthenticationAPI.Infrastructure.Data;
using ECommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationAPI.Infrastructure.Repositories
{
    public class UserRepository(AuthenticationDbContext context, IConfiguration config) : IUser
    {
        private async Task<AppUser> GetUserByEmail(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user is null ? null! : user;
        }

        public async Task<AppUserWithoutPasswordDTO> GetUser(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            return user is null ? null! : new AppUserWithoutPasswordDTO(
                user.Id, user.Name!, user.PhoneNumber!, user.Address!, user.Email!, user.Role!);
        }

        public async Task<Response> Login(LoginDTO loginDTO)
        {
            var user = await GetUserByEmail(loginDTO.Email);
            if (user != null)
            {
                return new Response(false, $"Invalid credentials.");
            }
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user!.Password);
            if (!isPasswordValid)
            {
                return new Response(false, $"Invalid credentials.");
            }
            string token = GenerateToken(user);
            return new Response(true, token);
        }

        private string GenerateToken(AppUser user)
        {
            var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Name!),
                new(ClaimTypes.Email, user.Email!)
            };
            if (!string.IsNullOrEmpty(user.Role) || !Equals("string", user.Role))
            {
                claims.Add(new(ClaimTypes.Role, user.Role!));
            }
            var token = new JwtSecurityToken(
                issuer: config["Authentication:Issuer"],
                audience: config["Authentication:Audience"],
                claims: claims,
                expires: null,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Response> Register(AppUserDTO appUserDTO)
        {
            var user = await GetUserByEmail(appUserDTO.Email);
            if (user != null)
            {
                return new Response(false, $"Email is already used for an existing user account.");
            }

            var result = context.Users.Add(new AppUser()
            {
                Name = appUserDTO.Name,
                PhoneNumber = appUserDTO.PhoneNumber,
                Address = appUserDTO.Address,
                Email = appUserDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(appUserDTO.Password),
                Role = appUserDTO.Role
            });
            await context.SaveChangesAsync();

            return result.Entity.Id > 0 ? new Response(true, $"User account successfuly created.")
                : new Response(false, "Unexpected error at user registration.");
        }
    }
}
