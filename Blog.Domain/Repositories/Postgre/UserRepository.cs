using Blog.Domain.Entities;
using Blog.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Domain.Repositories.Postgre;

public class UserRepository: IUserRepository
{
    private readonly AppDbContext _ctx;
    public UserRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<User> GetById(int id)
    {
        return await _ctx.Users.FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<User> PostUser(User user)
    {
        _ctx.Users.Add(user);
        await _ctx.SaveChangesAsync();

        return user;
    }
}