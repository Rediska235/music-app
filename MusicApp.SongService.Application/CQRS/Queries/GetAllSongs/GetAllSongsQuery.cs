using MediatR;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Queries.GetAllSongs;

public record GetAllSongsQuery : IRequest<IEnumerable<Song>>;