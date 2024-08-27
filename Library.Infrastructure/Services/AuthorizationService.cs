using System.Text;
using AutoInterfaceAttributes;
using Library.Core.DTO;
using Library.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Library.Infrastructure.DatabaseContext;
using Library.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Library.Infrastructure.Services;

[AutoInterface]
public class AuthorizationService(DataContext context, IConfiguration configuration) : IAuthorizationService
{
    public async Task<User> Login(string username, string password)
    {
        throw new NotImplementedException();
    }
    
    public async Task Register(RegisterUserRequest registerUserRequest)
    {
        throw new NotImplementedException();
    }
    
    public async Task<string> GenerateTokenAsync(string login, string password)
    {
        // Поиск пользователя по логину и паролю
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Login == login && u.Password == password);

        if (user == null)
        {
            throw new EntityNotFoundException("User not found or invalid credentials");
        }

        // Создание списка клеймов
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("id", user.Id.ToString()),
            new Claim("name", user.Login),
            new Claim("role", user.Role) // Добавляем роль пользователя в клеймы
        };

        // Подготовка ключа и подписывающей информации
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(10);

        // Создание JWT токена
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: creds
        );

        // Возвращаем сгенерированный токен
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}