using MediatR;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.CreateArtist;

public record CreateArtistCommand(string Username) : IRequest<Artist>;