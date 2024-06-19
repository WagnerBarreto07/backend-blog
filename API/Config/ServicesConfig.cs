using Application.Interfaces;
using Application.Services;

namespace API.Config;
public class ServicesConfig
{
    public ServicesConfig(IServiceCollection services)
    {
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<INotificacaoService, NotificacaoService>();
    }
}
