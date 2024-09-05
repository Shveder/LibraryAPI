var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();
builder.Services.AddCors(opt => opt.AddDefaultPolicy(b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();

#region Configure Services

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Controllers Configuration
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddAutoMapper(typeof(BookProfile));
builder.Services.AddAutoMapper(typeof(AuthorProfile));
builder.Services.AddAutoMapper(typeof(UserBookProfile));
builder.Services.AddAutoMapper(typeof(UserProfile));

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IDbRepository, DbRepository>();

// Custom Services Configuration
ServiceConfig.RegisterService(builder.Services);

#endregion

var app = builder.Build();

#region Middleware
app.UseCors();
// Static Files Configuration
var baseDirectory = Directory.GetParent(Environment.CurrentDirectory)?.ToString() ?? Environment.CurrentDirectory;
var filesFolderPath = Path.Combine(baseDirectory, "files");
Directory.CreateDirectory(filesFolderPath);

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(filesFolderPath),
    RequestPath = "/files"
});

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

app.UseRouting();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

#endregion

#region Map Endpoints

// Map Controller Routes
app.MapControllers();
app.MapDefaultControllerRoute();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AudioService-API V1");
});

#pragma warning disable ASP0014
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
#pragma warning restore ASP0014

#endregion

#region Initialize Database

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        context.Database.Migrate();
        await SeedDataAsync(context);

    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "An error occurred creating the DB.");
    }
}

#endregion

app.Run();

static async Task SeedDataAsync(DataContext context)
{
    if (await context.Users.AnyAsync()) return;
    await context.Users.AddAsync(new User{Login = "admin",
        Password = "0185b159c6bddbd7ec53cc96b3589f012e939d91f9c3476ac3f43bbd6ef46dce",
        Salt = "sZdDpAEbpBCPyztnn4BqcQ==", Role = "admin"});
    await context.SaveChangesAsync();
}