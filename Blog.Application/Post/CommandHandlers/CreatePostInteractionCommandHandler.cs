using Blog.Application.Post.Commands;
using Blog.DAL;
using Blog.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Post.CommandHandlers;

public class CreatePostInteractionCommandHandler: IRequestHandler<CreatePostInteractionCommand, PostInteraction?>
{
    private readonly DataContext _ctx;
    
    public CreatePostInteractionCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<PostInteraction?> Handle(CreatePostInteractionCommand request, CancellationToken cancellationToken)
    {
        var post = await _ctx.Posts.FirstOrDefaultAsync(post => post.Id == request.PostId, cancellationToken);

        if (post == null)
        {
            return null;
        }

        var interaction = PostInteraction.CreatePostInteraction(request.PostId, request.Type);
        post.AddInteraction(interaction);

        _ctx.Posts.Update(post);

        await _ctx.SaveChangesAsync(cancellationToken);

        return interaction;
    }
}