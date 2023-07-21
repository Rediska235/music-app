using MediatR;

namespace MusicApp.SongService.Application.CQRS.Commands.DeleteSong;

public record DeleteSongCommand(Guid Id) : IRequest;