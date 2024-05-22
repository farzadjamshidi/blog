using Blog.Domain.Entities;

namespace Blog.Domain.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> GetById(int id);
    Task<User> PostUser(User user);
}