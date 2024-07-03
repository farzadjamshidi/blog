using MediatR;

namespace Blog.Application.Post.Commands;

public class CreatePostCommand : IRequest<Domain.Aggregates.PostAggregate.Post>
{ 
    public Guid UserProfileId { get; set; }
    public string Text { get; private set; }
}