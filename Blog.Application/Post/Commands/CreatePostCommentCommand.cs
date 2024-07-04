using Blog.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Blog.Application.Post.Commands;

public class CreatePostCommentCommand : IRequest<PostComment?>
{
    public Guid PostId { get; set; }
    public Guid UserProfileId { get; set; }
    public string Text { get; set; }

}