using MediatR;
using MusicApp.Shared;

namespace MusicApp.SongService.Application.CQRS.Commands.AddArtist;

public record AddArtistCommand(UserPublishedDto UserPublishedDto) : IRequest;
