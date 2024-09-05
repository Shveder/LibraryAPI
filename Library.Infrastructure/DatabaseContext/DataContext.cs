namespace Library.Infrastructure.DatabaseContext;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    
    public DbSet<Author> Authors { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<UserBook> UserBooks { get; set; }
}