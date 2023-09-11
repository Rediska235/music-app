using MediatR;
using MusicApp.SongService.Application.DTOs;

namespace MusicApp.SongService.Application.CQRS.Queries.GetFavoriteSongs;

public record GetFavoriteSongsQuery() : IRequest<IEnumerable<SongOutputDto>>;