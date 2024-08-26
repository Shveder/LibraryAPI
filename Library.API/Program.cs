using System.Reflection;
using System.Text.Json;
using Library.Infrastructure;
using Library.Infrastructure.DatabaseContext;
using Library.Infrastructure.Mappings;
using Library.Infrastructure.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Routing Configuration
builder.Services.AddRouting(opt =>
{
    opt.LowercaseUrls = true;
    opt.LowercaseQueryStrings = true;
});

// Custom Services Configuration
ServiceConfig.RegisterService(builder.Services);

#endregion


var app = builder.Build();

#region Middleware

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

// Ensure Database Created and Apply Migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dbContext.Database.EnsureCreated();
}

#endregion


app.Run();