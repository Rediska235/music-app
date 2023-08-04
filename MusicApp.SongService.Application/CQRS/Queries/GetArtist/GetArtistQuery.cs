using MediatR;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Queries.GetArtist;

public record GetArtistQuery() : IRequest<Artist>;