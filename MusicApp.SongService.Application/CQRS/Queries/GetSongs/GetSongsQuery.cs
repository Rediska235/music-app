using MediatR;
using MusicApp.SongService.Application.DTOs;

namespace MusicApp.SongService.Application.CQRS.Queries.GetSongs;

public record GetSongsQuery : IRequest<IEnumerable<SongOutputDto>>;