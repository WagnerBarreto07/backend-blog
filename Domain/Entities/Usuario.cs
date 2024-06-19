
namespace Domain.Entities;
public class Usuario
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public DateTime UltimoLogin { get; set; }

    public ICollection<Post> Posts { get; } = new List<Post>();
}
