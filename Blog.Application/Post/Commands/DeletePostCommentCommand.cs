using Blog.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Blog.Application.Post.Commands;

public class DeletePostCommentCommand : IRequest<PostComment?>
{
    public Guid PostId { get; set; }
    public Guid CommentId { get; set; }

}