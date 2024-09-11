namespace Library.Application.UseCases.AuthorizationUseCases;

[AutoInterface]
public class LoginUseCase(IConfiguration configuration, 
    IDbRepository repository) : BaseAuthorization, ILoginUseCase
{
    public async Task<string> GenerateTokenAsync(string login, string password)
    {
        var user1 = await repository.Get<User>(model => model.Login == login).FirstOrDefaultAsync();

        if (user1 == null)
            throw new IncorrectDataException("Invalid login or password");

        password = Hash(password);
        password = Hash(password + user1.Salt);

        var user = await repository.Get<User>(u =>
            u.Login == login && u.Password == password).FirstOrDefaultAsync();

        if (user == null)
            throw new EntityNotFoundException("User not found or invalid credentials");
        
        var claims = new List<Claim>
        {
            new ("id", user.Id.ToString()),
            new ("name", user.Login),
            new ("role", user.Role)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: signIn));
    }
}