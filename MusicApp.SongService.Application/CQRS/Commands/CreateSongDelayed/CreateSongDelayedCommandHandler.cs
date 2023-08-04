using AutoMapper;
using Hangfire;
using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;
using System.Linq.Expressions;

namespace MusicApp.SongService.Application.CQRS.Commands.CreateSongDelayed;

public class CreateSongDelayedCommandHandler : IRequestHandler<CreateSongDelayedCommand>
{
    private readonly ISongRepository _songRepository;
    private readonly IArtistRepository _artistRepository;
    private readonly IMapper _mapper;

    public CreateSongDelayedCommandHandler(ISongRepository songRepository, IArtistRepository artistRepository, IMapper mapper)
    {
        _songRepository = songRepository;
        _artistRepository = artistRepository;
        _mapper = mapper;
    }

    public async Task Handle(CreateSongDelayedCommand request, CancellationToken cancellationToken)
    {
        var song = _mapper.Map<Song>(request.delayedSongInputDto);
        var artistName = request.Artist.Username;

        var job = (Expression<Func<Task>>)(() => AddToDatabase(song, artistName, cancellationToken));

        var delay = request.delayedSongInputDto.PublishTime - DateTime.Now;

        BackgroundJob.Schedule(job, delay);
    }

    public async Task AddToDatabase(Song song, string artistName, CancellationToken cancellationToken)
    {
        var artist = await _artistRepository.GetArtistByUsernameAsync(artistName, cancellationToken);
        if(artist != null)
        {
            song.Artist = artist;
        }

        await _songRepository.CreateAsync(song, cancellationToken);
        await _songRepository.SaveChangesAsync(cancellationToken);
    }
}