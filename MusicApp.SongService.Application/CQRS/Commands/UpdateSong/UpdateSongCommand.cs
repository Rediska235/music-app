using MediatR;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.UpdateSong;

public record UpdateSongCommand(Song Song) : IRequest<Song>;