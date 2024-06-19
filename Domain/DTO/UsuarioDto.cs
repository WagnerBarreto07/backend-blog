using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;
public class UsuarioDto
{
    [Key]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("nome")]
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MaxLength(100)]
    [MinLength(5)]
    public string Nome { get; set; } = string.Empty;

    [JsonProperty("email")]
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
    [MaxLength(50)]
    [MinLength(5)]
    public string Email { get; set; } = string.Empty;

    [JsonProperty("senha")]
    [Required(ErrorMessage = "A senha é obrigatória")]
    [MaxLength(15)]
    [MinLength(6)]
    public string Senha { get; set; } = string.Empty;   
}
