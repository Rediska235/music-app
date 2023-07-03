using MediatR;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Queries.GetSongById;

public record GetSongByIdQuery(Guid Id) : IRequest<Song>;