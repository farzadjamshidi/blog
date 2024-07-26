using Blog.Domain.Aggregates.PostAggregate;

namespace Blog.Application.Dtos.Post;

public class GetPostByIdDtoApp
{
    public Blog.Domain.Aggregates.PostAggregate.Post Post { get; set; }
    public List<InteractionCount> InteractionsCount { get; set; }
}

public class InteractionCount
{
    public InteractionType Type { get; set; }
    public int Count { get; set; }
}