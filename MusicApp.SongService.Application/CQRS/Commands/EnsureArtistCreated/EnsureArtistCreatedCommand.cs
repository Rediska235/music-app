using MediatR;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.EnsureArtistCreated;

public record EnsureArtistCreatedCommand() : IRequest<Artist>;