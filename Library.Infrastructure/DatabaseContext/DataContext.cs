using Library.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DatabaseContext;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            @"Host=localhost;Port=5432;Database=library;Username=postgres;Password=postgres");
    }
}