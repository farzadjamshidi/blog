using MediatR;

namespace Blog.Application.Post.Commands;

public class UpdatePostCommand: IRequest<Domain.Aggregates.PostAggregate.Post?>
{
    public Guid Id { get; set; }
    public string Text { get; set; }
}