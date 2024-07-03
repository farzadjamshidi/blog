using MediatR;

namespace Blog.Application.Post.Queries;

public class GetAllPostQuery: IRequest<IEnumerable<Domain.Aggregates.PostAggregate.Post>>
{
    
}