using Blog.Domain.Aggregates.PostAggregate;

namespace Blog.API.Dtos.V1.Post.Requests;

public class CreatePostInteractionDtoReq
{
    public InteractionType Type { get; set; }
}