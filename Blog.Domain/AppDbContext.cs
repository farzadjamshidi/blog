using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Domain;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users {get; set; }
    public DbSet<Post> Posts {get; set; }
    public DbSet<Comment> Comments { get; set; }
}