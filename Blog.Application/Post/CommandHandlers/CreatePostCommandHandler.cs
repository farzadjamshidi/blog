using Blog.Application.Post.Commands;
using Blog.DAL;
using MediatR;

namespace Blog.Application.Post.CommandHandlers;

public class CreatePostCommandHandler: IRequestHandler<CreatePostCommand, Domain.Aggregates.PostAggregate.Post>
{
    private readonly DataContext _ctx;
    
    public CreatePostCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Domain.Aggregates.PostAggregate.Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = Domain.Aggregates.PostAggregate.Post.CreatePost(request.UserProfileId, request.Text);

        _ctx.Posts.Add(post);
        await _ctx.SaveChangesAsync(cancellationToken);

        return post;
    }
}