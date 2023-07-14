using MediatR;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.UpdateSong;

public record UpdateSongCommand(Guid Id, SongInputDto Song) : IRequest<Song>;