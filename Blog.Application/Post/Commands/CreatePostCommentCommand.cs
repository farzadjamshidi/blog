using Blog.Application.Dtos.Post;
using MediatR;

namespace Blog.Application.Post.Commands;

public class CreatePostCommentCommand : IRequest<CreatePostCommentResult?>
{
    public Guid PostId { get; set; }
    public Guid UserProfileId { get; set; }
    public string Text { get; set; }

}