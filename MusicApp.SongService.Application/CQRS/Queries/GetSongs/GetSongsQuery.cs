using MediatR;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Queries.GetSongs;

public record GetSongsQuery : IRequest<IEnumerable<Song>>;