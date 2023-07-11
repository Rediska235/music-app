using MediatR;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.DeleteSong;

public record DeleteSongCommand(Guid Id) : IRequest<Song>;