using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using NoughtsAndCrosses.Core.Infrastructure.Domain;

namespace NoughtsAndCrosses.Core.Infrastructure;

public abstract class WebSocketCommandBase
{
    public abstract Task Execute(WebSocket client);
}