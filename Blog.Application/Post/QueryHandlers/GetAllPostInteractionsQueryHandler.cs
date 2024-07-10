using Blog.Application.Post.Queries;
using Blog.DAL;
using Blog.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Post.QueryHandlers;

public class GetAllPostInteractionsQueryHandler: IRequestHandler<GetAllPostInteractionsQuery, IEnumerable<PostInteraction>?>
{
    private readonly DataContext _ctx;
    
    public GetAllPostInteractionsQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<IEnumerable<PostInteraction>?> Handle(GetAllPostInteractionsQuery request, CancellationToken cancellationToken)
    {
        var post = await _ctx.Posts
            .Include(post => post.Interactions)
            .FirstOrDefaultAsync(post => post.Id == request.PostId, cancellationToken);

        return post?.Interactions;
    }
}