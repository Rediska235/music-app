using MediatR;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Queries.GetArtistByUsername;

public record GetArtistByUsernameQuery(string Username) : IRequest<Artist>;