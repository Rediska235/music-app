using MassTransit;
using MediatR;
using MusicApp.Shared;
using MusicApp.SongService.Application.CQRS.Commands.AddArtist;

namespace MusicApp.SongService.Web.MessageBroker;

public class UserConsumer : IConsumer<UserPublishedDto>
{
    private readonly IMediator _mediator;

    public UserConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<UserPublishedDto> context)
    {
        var command = new AddArtistCommand(context.Message);
        await _mediator.Send(command, new CancellationToken());
    }
}