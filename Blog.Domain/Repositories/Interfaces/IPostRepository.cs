using Blog.Domain.Entities;

namespace Blog.Domain.Repositories.Interfaces;

public interface IPostRepository
{
    Task<Post> GetById(int id);
    Task<Post> PostPost(int authorId, Post post);
}