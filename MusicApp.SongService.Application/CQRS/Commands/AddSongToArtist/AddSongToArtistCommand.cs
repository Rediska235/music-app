using MediatR;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.UpdateArtist;

public record AddSongToArtistCommand(Song Song, Artist Artist) : IRequest<Artist>;