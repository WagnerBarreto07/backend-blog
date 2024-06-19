
using Domain.Entities;
using Infra.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infra.Context;
public class BlogContext : DbContext
{
    public BlogContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());

        modelBuilder.Entity<Usuario>()
        .HasMany(e => e.Posts)
        .WithOne(e => e.Usuario)
        .HasForeignKey(e => e.UserId)
        .IsRequired().OnDelete(DeleteBehavior.ClientNoAction);
    }

    public virtual DbSet<Usuario> Usuarios { get; set; }   
    public virtual DbSet<Post> Posts { get; set; }
}
