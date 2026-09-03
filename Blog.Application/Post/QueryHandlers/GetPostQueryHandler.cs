using Blog.Application.Dtos.Post;
using Blog.Application.Post.Queries;
using Blog.DAL;
using Blog.Domain.Aggregates.PostAggregate.Reactions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Post.QueryHandlers;

public class GetPostQueryHandler(DataContext ctx): IRequestHandler<GetPostQuery, GetPostByIdDtoApp?>
{
    public async Task<GetPostByIdDtoApp?> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
        var dto = await ctx.Posts
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

        if (dto != null)
        {
            foreach (var interactionCount in dto.InteractionsCount)
            {
                interactionCount.Weight = Reaction.FromType(interactionCount.Type).Weight;
            }

            dto.EngagementScore = dto.InteractionsCount.Sum(ic => ic.Count * ic.Weight);
        }

        return dto;
    }
}