using Blog.Domain.Entities;

namespace Blog.Domain.Repositories.Interfaces;

public interface ICommentRepository
{
    Task<Comment> GetById(int id);
    Task<Comment> Delete(int id);
    Task<Comment> Post(int postId, Comment comment);
}