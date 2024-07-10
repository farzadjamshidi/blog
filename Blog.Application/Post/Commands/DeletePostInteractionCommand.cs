using Blog.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Blog.Application.Post.Commands;

public class DeletePostInteractionCommand : IRequest<PostInteraction?>
{
    public Guid PostId { get; set; }
    public Guid InteractionId { get; set; }
}