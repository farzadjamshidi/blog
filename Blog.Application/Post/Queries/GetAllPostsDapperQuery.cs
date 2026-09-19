using Blog.Application.Dtos.Post;
using MediatR;

namespace Blog.Application.Post.Queries;

public class GetAllPostsDapperQuery : IRequest<IEnumerable<PostSummaryDapperDto>>
{
}
