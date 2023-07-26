using MassTransit;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.Shared;

namespace MusicApp.PlaylistService.Web.MessageBroker;

public class SongConsumer : IConsumer<SongMessage>
{
    private readonly ISongService _service;

    public SongConsumer(ISongService service)
    {
        _service = service;
    }

    public async Task Consume(ConsumeContext<SongMessage> context)
    {
        switch(context.Message.Operation)
        {
            case Operation.Created:
                await _service.AddSongAsync(context.Message.Song, new CancellationToken());
                break;

            case Operation.Updated:
                await _service.UpdateSongAsync(context.Message.Song, new CancellationToken());
                break;

            case Operation.Deleted:
                await _service.RemoveSongAsync(context.Message.Song, new CancellationToken());
                break;
        }

    }
}
