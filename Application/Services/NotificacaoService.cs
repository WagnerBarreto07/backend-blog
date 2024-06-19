
using Application.Interfaces;
using System.Net.WebSockets;
using System.Text;

namespace Application.Services;
public class NotificacaoService : INotificacaoService
{
    public async Task<string> EnviaNotificacaoAsync()
    {
        using var ws = new ClientWebSocket();

        await ws.ConnectAsync(new Uri("ws://localhost:5210"), CancellationToken.None);

        var buffer = new byte[256];

        if (ws.State == WebSocketState.Open)
        {
            var result = await ws.ReceiveAsync(buffer, CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
            else
                return (Encoding.ASCII.GetString(buffer, 0, result.Count));

        }

        return "";
    }
}
