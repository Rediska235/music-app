using MediatR;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.CreateSongDelayed;

public record CreateSongDelayedCommand(DelayedSongInputDto delayedSongInputDto, Artist Artist) : IRequest;
