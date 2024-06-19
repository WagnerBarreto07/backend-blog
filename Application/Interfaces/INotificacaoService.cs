
namespace Application.Interfaces;
public interface INotificacaoService
{
    Task<string> EnviaNotificacaoAsync();
}
