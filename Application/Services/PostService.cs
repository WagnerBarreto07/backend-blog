
using Application.Interfaces;
using Domain.Entities;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
public class PostService : IPostService
{
    private readonly BlogContext _context;
    public PostService(BlogContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }    

    public async Task UpdateAsync(Post post)
    {
       _context.Posts.Update(post);
       await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(c => c.Id == id);

        if (post == null) 
        { 
            return false;
        }
        else
        {
            _context.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public async Task<List<Post>> GetAllPostsAsync()
    {
        return await _context.Posts.AsNoTracking().ToListAsync();
    }   
}
