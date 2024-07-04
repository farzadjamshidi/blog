using Blog.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Blog.Application.Post.Queries;

public class GetAllPostCommentQuery : IRequest<IEnumerable<PostComment>?>
{
    public Guid PostId { get; set; }
}