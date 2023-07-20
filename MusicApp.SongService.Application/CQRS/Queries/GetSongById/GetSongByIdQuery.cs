using MediatR;
using MusicApp.SongService.Application.DTOs;

namespace MusicApp.SongService.Application.CQRS.Queries.GetSongById;

public record GetSongByIdQuery(Guid Id) : IRequest<SongOutputDto>;