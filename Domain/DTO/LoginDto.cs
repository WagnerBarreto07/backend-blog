
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;
public class LoginDto
{
    [JsonProperty("email")]
    [Required(ErrorMessage = "O {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "O {0} deve ter entre {2} e {1} caracteres")]
    public string Email { get; set; } = string.Empty;

    [JsonProperty("senha")]
    [Required(ErrorMessage = "O {0} é obrigatório")]
    [StringLength(15, MinimumLength = 6, ErrorMessage = "O {0} deve ter entre {2} e {1} caracteres")]
    public string Senha { get; set; } = string.Empty;
}
