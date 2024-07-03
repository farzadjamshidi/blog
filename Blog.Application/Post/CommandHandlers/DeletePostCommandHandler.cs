using Blog.Application.Post.Commands;
using Blog.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Post.CommandHandlers;

public class DeletePostCommandHandler: IRequestHandler<DeletePostCommand, Domain.Aggregates.PostAggregate.Post?>
{
    private readonly DataContext _ctx;
    
    public DeletePostCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Domain.Aggregates.PostAggregate.Post?> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _ctx.Posts.FirstOrDefaultAsync(post => post.Id == request.Id, cancellationToken);

        if (post == null)
        {
            return null;
        }

        _ctx.Posts.Remove(post);
        await _ctx.SaveChangesAsync(cancellationToken);

        return post;
    }
}