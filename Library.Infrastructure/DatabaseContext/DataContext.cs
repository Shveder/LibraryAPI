using Library.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DatabaseContext;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
}