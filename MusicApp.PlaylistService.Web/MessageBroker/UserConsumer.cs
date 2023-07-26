using MassTransit;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.Shared;

namespace MusicApp.PlaylistService.Web.MessageBroker;

public class UserConsumer : IConsumer<UserPublishedDto>
{
    private readonly IUserService _service;

    public UserConsumer(IUserService service)
    {
        _service = service;
    }

    public async Task Consume(ConsumeContext<UserPublishedDto> context)
    {
        await _service.AddUserAsync(context.Message, new CancellationToken());
    }
}