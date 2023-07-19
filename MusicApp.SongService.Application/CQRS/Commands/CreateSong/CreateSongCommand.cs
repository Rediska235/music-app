using MediatR;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.CreateSong;

public record CreateSongCommand(SongInputDto Song, Artist Artist) : IRequest<Song>;
