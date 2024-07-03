using Blog.Application.Post.Queries;
using Blog.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Post.QueryHandlers;

public class GetPostQueryHandler: IRequestHandler<GetPostQuery, Domain.Aggregates.PostAggregate.Post?>
{
    private readonly DataContext _ctx;
    
    public GetPostQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Domain.Aggregates.PostAggregate.Post?> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
        return await _ctx.Posts.FirstOrDefaultAsync(post => post.Id == request.Id, cancellationToken);
    }
}