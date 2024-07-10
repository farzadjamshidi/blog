using Blog.Application.Post.Commands;
using Blog.DAL;
using Blog.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Post.CommandHandlers;

public class DeletePostInteractionCommandHandler: IRequestHandler<DeletePostInteractionCommand, PostInteraction?>
{
    private readonly DataContext _ctx;
    
    public DeletePostInteractionCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<PostInteraction?> Handle(DeletePostInteractionCommand request, CancellationToken cancellationToken)
    {
        var post = await _ctx.Posts
            .Include(post => post.Interactions.Where(ia => ia.Id == request.InteractionId))
            .FirstOrDefaultAsync(post => post.Id == request.PostId, cancellationToken);

        if (post == null)
        {
            return null;
        }

        var interaction = post.Interactions.FirstOrDefault(ia => ia.Id == request.InteractionId);
        
        post.RemoveInteraction(interaction);

        _ctx.Posts.Update(post);
        await _ctx.SaveChangesAsync(cancellationToken);

        return interaction;
    }
}