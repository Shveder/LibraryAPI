﻿namespace Library.Application.UseCases.AuthorizationUseCases;

public class RegisterUseCase(IMapper mapper, IDbRepository repository, ILogger<RegisterUseCase> logger)
{
    public async Task Register(RegisterUserRequest request)
    {
        if (request.Password != request.PasswordRepeat)
            throw new IncorrectDataException("Passwords do not match");
        
        if (await IsLoginUnique(request.Login))
            throw new IncorrectDataException("There is already a user with this login in the system");
        
        if (request.Login.Length is < 4 or > 32)
            throw new IncorrectDataException("Login length must be between 4 and 32 characters.");
        
        if (request.Password.Length is < 4 or > 32)
            throw new IncorrectDataException("Password length must be between 4 and 32 characters.");

        request.Password = Hash(request.Password);
        string salt = GetSalt();
        request.Password = Hash(request.Password + salt);

        var user = mapper.Map<User>(request);
        user.Password = request.Password;
        user.Salt = salt;
        user.Role = "user"; 

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
        var rng = new RNGCryptoServiceProvider();
        rng.GetBytes(salt);
        
        return Convert.ToBase64String(salt);
    }

    private string Hash(string inputString)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
            
            return sb.ToString();
        }
    }
}