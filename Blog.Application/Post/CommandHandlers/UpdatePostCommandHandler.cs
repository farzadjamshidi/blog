using Blog.Application.Post.Commands;
using Blog.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Post.CommandHandlers;

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Domain.Aggregates.PostAggregate.Post?>
{
    private readonly DataContext _ctx;
    
    public UpdatePostCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Domain.Aggregates.PostAggregate.Post?> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _ctx.Posts.FirstOrDefaultAsync(post => post.Id == request.Id, cancellationToken);

        if (post == null)
        {
            return null;
        }
        
        post.UpdatePostText(request.Text);

        _ctx.Posts.Update(post);

        await _ctx.SaveChangesAsync(cancellationToken);

        return post;
    }
}