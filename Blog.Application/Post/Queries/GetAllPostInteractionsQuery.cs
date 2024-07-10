using Blog.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Blog.Application.Post.Queries;

public class GetAllPostInteractionsQuery : IRequest<IEnumerable<PostInteraction>?>
{
    public Guid PostId { get; set; }
}