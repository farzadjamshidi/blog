using Blog.Application.Post.Queries;
using Blog.DAL;
using Blog.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Post.QueryHandlers;

public class GetAllPostCommentQueryHandler : IRequestHandler<GetAllPostCommentQuery, IEnumerable<PostComment>?>
{
    private readonly DataContext _ctx;
    
    public GetAllPostCommentQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<IEnumerable<PostComment>?> Handle(GetAllPostCommentQuery request, CancellationToken cancellationToken)
    {
        var post = await _ctx.Posts
            .Include(post => post.Comments)
            .FirstOrDefaultAsync(post => post.Id == request.PostId, cancellationToken);

        if (post == null)
        {
            return null;
        }

        return post.Comments;
    }
}