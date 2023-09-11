using MediatR;
using MusicApp.SongService.Application.DTOs;

namespace MusicApp.SongService.Application.CQRS.Commands.UpdateSong;

public record UpdateSongCommand(Guid Id, SongInputDto Song) : IRequest<SongOutputDto>;
