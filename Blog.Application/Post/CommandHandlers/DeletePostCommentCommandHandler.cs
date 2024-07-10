using Blog.Application.Post.Commands;
using Blog.DAL;
using Blog.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Post.CommandHandlers;

public class DeletePostCommentCommandHandler: IRequestHandler<DeletePostCommentCommand, PostComment?>
{
    private readonly DataContext _ctx;
    
    public DeletePostCommentCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<PostComment?> Handle(DeletePostCommentCommand request, CancellationToken cancellationToken)
    {
        var post = await _ctx.Posts
            .Include(post => post.Comments.Where(c => c.Id == request.CommentId))
            .FirstOrDefaultAsync(post => post.Id == request.PostId, cancellationToken);

        if (post == null)
        {
            return null;
        }

        var comment = post.Comments.FirstOrDefault(ia => ia.Id == request.CommentId);
        
        post.RemoveComment(comment);

        _ctx.Posts.Update(post);
        await _ctx.SaveChangesAsync(cancellationToken);

        return comment;
    }
}