using Blog.Domain.Aggregates.PostAggregate;

namespace Blog.API.Dtos.V1.Post.Responses;

public class CreatePostInteractionDtoRes
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public InteractionType Type { get; set; }
}