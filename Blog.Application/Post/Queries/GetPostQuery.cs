using Blog.Application.Dtos.Post;
using MediatR;

namespace Blog.Application.Post.Queries;

public class GetPostQuery: IRequest<GetPostByIdDtoApp?>
{
    public Guid Id { get; set; }
}