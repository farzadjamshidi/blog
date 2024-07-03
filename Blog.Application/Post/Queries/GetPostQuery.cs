using MediatR;

namespace Blog.Application.Post.Queries;

public class GetPostQuery: IRequest<Domain.Aggregates.PostAggregate.Post?>
{
    public Guid Id { get; set; }
}