using Microsoft.AspNetCore.SignalR;

namespace MusicApp.SongService.Web.Hubs;

public class SongHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId}, has joined");
    }

    public async Task Send(string username, string songTitle)
    {
        await Clients.All.SendAsync("ReceiveMessage", $"{username} added song {songTitle}");
    }
}
