
using Domain.Entities;

namespace Application.Interfaces;
public interface IPostService
{
    Task CreateAsync(Post post);

    Task UpdateAsync(Post post);

    Task<bool> DeleteAsync(int id);

    Task<List<Post>> GetAllPostsAsync();
}
