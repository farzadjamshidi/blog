using Blog.Domain.Entities;
using Blog.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Domain.Repositories.Postgre;

public class CommentRepository: ICommentRepository
{
    private readonly AppDbContext _ctx;
    public CommentRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Comment> GetById(int id)
    {
        return await _ctx.Comments.FirstOrDefaultAsync(comment => comment.Id == id);
    }
    
    public async Task<Comment> Delete(int id)
    {
        var comment = await _ctx.Comments.FirstOrDefaultAsync(comment => comment.Id == id);
        
        _ctx.Comments.Remove(comment);
        await _ctx.SaveChangesAsync();

        return comment;
    }

    public async Task<Comment> Post(int postId, Comment comment)
    {
        var post = await _ctx.Posts
            .Include(post => post.Comments)
            .FirstOrDefaultAsync(post => post.Id == postId);
        
        if (post == null) 
            return null;
        
        post.Comments.Add(comment);
        await _ctx.SaveChangesAsync();

        return comment;
    }
}