using MassTransit;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.Shared;

namespace MusicApp.PlaylistService.Web.MessageBroker;

public class SongConsumer : IConsumer<SongPublishedDto>
{
    private readonly ISongService _service;

    public SongConsumer(ISongService service)
    {
        _service = service;
    }

    public async Task Consume(ConsumeContext<SongPublishedDto> context)
    {
        await _service.AddSongAsync(context.Message, new CancellationToken());
    }
}
