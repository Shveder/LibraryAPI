using System.Text;
using AutoInterfaceAttributes;
using Library.Core.DTO;
using Library.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Library.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Library.Infrastructure.DatabaseContext;
using Library.Infrastructure.Repository;

namespace Library.Infrastructure.Services;

[AutoInterface]
public class AuthorizationService(DataContext context, IConfiguration configuration,
    ILogger<AuthorizationService>logger, IDbRepository repository) : IAuthorizationService
{
    public async Task<User> Login(string login, string password)
    {
        var user = await repository.Get<User>(model => model.Login == login).FirstOrDefaultAsync();
        
        if (user == null)
            throw new IncorrectDataException("Invalid login or password");
        
        password = Hash(password);
        password = Hash(password + user.Salt);

        var user1 = await repository.Get<User>(model =>
            model.Login == login && model.Password == password).FirstOrDefaultAsync();

        if (user1 == null)
            throw new IncorrectDataException("Invalid login or password");

        return user1;
    }
    
    public async Task Register(RegisterUserRequest request)
    {
        if (request.Password != request.PasswordRepeat)
            throw new IncorrectDataException("Passwords do not match");
        if(await IsLoginUnique(request.Login))
            throw new IncorrectDataException("There is already a user with this login in the system");
        if(request.Login.Length is < 4 or > 32)
            throw new IncorrectDataException("Login length must be between 4 and 32 characters.");
        if(request.Password.Length is < 4 or > 32)
            throw new IncorrectDataException("Password length must be between 4 and 32 characters.");
        
        request.Password = Hash(request.Password);
        string salt = GetSalt();
        request.Password = Hash(request.Password + salt);
        
        var user = new User
        {
            Login = request.Login,
            Password = request.Password,
            Salt = salt,
            Role = "user"
        };
        await repository.Add(user);
        await repository.SaveChangesAsync();
        logger.LogInformation($"User created (Login: {request.Login})");
    }
    private async Task<bool> IsLoginUnique(string login)
    {
        var user = await repository.Get<User>(model => model.Login == login).FirstOrDefaultAsync(); 
        return user != null;
    }
    private string GetSalt()
    {
        byte[] salt = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        return Convert.ToBase64String(salt);
    }

    private string Hash(string inputString)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
    public async Task<string> GenerateTokenAsync(string login, string password)
    {
        var user1 = await context.Set<User>().FirstOrDefaultAsync(model => model.Login == login);
        
        if (user1 == null)
            throw new IncorrectDataException("Invalid login or password");
        
        password = Hash(password);
        password = Hash(password + user1.Salt);
        
        var user = await repository.Get<User>(u =>
                u.Login == login && u.Password == password).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new EntityNotFoundException("User not found or invalid credentials");
        }

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("id", user.Id.ToString()),
            new Claim("name", user.Login),
            new Claim("role", user.Role) 
        };

      
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(10);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}