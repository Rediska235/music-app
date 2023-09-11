using MediatR;

namespace MusicApp.SongService.Application.CQRS.Commands.LikeSong;

public record LikeSongCommand(Guid Id) : IRequest;