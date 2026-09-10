using Blog.Application.Dtos.Post;
using Blog.Application.Post.Commands;
using Blog.DAL;
using Blog.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Post.CommandHandlers;

public class CreatePostCommentCommandHandler: IRequestHandler<CreatePostCommentCommand, CreatePostCommentResult?>
{
    private readonly DataContext _ctx;

    public CreatePostCommentCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<CreatePostCommentResult?> Handle(CreatePostCommentCommand request, CancellationToken cancellationToken)
    {
        var post = await _ctx.Posts.FirstOrDefaultAsync(post => post.Id == request.PostId, cancellationToken);

        if (post == null)
        {
            return null;
        }

        var comment = PostComment.CreatePostComment(request.PostId, request.UserProfileId, request.Text);
        post.AddComment(comment);

        _ctx.Posts.Update(post);

        await _ctx.SaveChangesAsync(cancellationToken);

        return new CreatePostCommentResult
        {
            Comment = comment,
            PostAuthorUserProfileId = post.UserProfileId
        };
    }
}