
namespace Domain.Entities;
public class AlterarSenha
{
    public int Id { get; set; }

    public string SenhaAtual { get; set; } = string.Empty;

    public string NovaSenha { get; set; } = string.Empty;
}
