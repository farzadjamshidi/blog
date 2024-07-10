using Blog.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Blog.Application.Post.Commands;

public class CreatePostInteractionCommand : IRequest<PostInteraction?>
{
    public Guid PostId { get; set; }

    public InteractionType Type { get; set; }
}