using MediatR;

namespace Blog.Application.Post.Commands;

public class DeletePostCommand: IRequest<Domain.Aggregates.PostAggregate.Post?>
{
    public Guid Id { get; set; }
}