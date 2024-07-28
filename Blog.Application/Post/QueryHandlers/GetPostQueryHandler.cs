using Blog.Application.Dtos.Post;
using Blog.Application.Post.Queries;
using Blog.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Post.QueryHandlers;

public class GetPostQueryHandler: IRequestHandler<GetPostQuery, GetPostByIdDtoApp?>
{
    private readonly DataContext _ctx;
    
    public GetPostQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<GetPostByIdDtoApp?> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
        return await _ctx.Posts
            .Where(post => post.Id == request.Id)
            .Include(post => post.UserProfile)
            .Include(post => post.Comments)
            .Include("Comments.UserProfile")
            .Select(p => new GetPostByIdDtoApp
            {
                Post = p,
                InteractionsCount = p.Interactions
                    .GroupBy(i => i.Type)
                    .Select(g => new InteractionCount
                    {
                        Type = g.Key,
                        Count = g.Count()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}