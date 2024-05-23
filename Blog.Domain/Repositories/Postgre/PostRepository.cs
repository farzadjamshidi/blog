using Blog.Domain.Entities;
using Blog.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Domain.Repositories.Postgre;

public class PostRepository: IPostRepository
{
    private readonly AppDbContext _ctx;
    public PostRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Post> GetById(int id)
    {
        return await _ctx.Posts.FirstOrDefaultAsync(post => post.Id == id);
    }

    public async Task<Post> PostPost(int authorId, Post post)
    {
        var user = await _ctx.Users
            .Include(user => user.Posts)
            .FirstOrDefaultAsync(user => user.Id == authorId);
        
        if (user == null) 
            return null;
        
        user.Posts.Add(post);
        await _ctx.SaveChangesAsync();

        return post;
    }
}