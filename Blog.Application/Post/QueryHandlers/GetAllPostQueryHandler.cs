using Blog.Application.Post.Queries;
using Blog.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Post.QueryHandlers;

public class GetAllPostQueryHandler: IRequestHandler<GetAllPostQuery, IEnumerable<Domain.Aggregates.PostAggregate.Post>>
{
    private readonly DataContext _ctx;
    
    public GetAllPostQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<IEnumerable<Domain.Aggregates.PostAggregate.Post>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
    {
        return await _ctx.Posts
            .Include(post => post.UserProfile)
            .ToListAsync(cancellationToken);
    }
}