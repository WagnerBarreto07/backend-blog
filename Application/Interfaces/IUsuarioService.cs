
using Domain.DTO;
using Domain.Entities;

namespace Application.Interfaces;
public interface IUsuarioService
{
    Task<Usuario> GetUserAsync(string email, string senha);

    Task<bool> CreateAsync(UsuarioDto user);

    Task UpdateAsync(Usuario user);

    Task UltimoLoginAsync(Usuario user);

    Task<bool> DeleteAsync(int id);

    Task<bool> UsuarioCadastradoAsync(UsuarioDto user);

    Task<bool> AlterarSenhaAsync(int id, string senhaAtual, string novaSenha);
}
