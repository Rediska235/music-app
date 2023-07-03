using MediatR;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.CreateSong;

public record CreateSongCommand(Song Song, Artist Artist) : IRequest<Song>;