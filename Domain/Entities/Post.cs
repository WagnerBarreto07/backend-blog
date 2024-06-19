
using System.Text.Json.Serialization;

namespace Domain.Entities;
public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;    
    public DateTime DataCriacao { get; set; }

    [JsonIgnore]
    public virtual Usuario? Usuario { get; set; }     
}