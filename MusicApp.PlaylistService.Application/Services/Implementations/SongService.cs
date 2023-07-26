using AutoMapper;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.Shared;

namespace MusicApp.PlaylistService.Application.Services.Implementations;

public class SongService : ISongService
{
    private readonly ISongRepository _songRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public SongService(ISongRepository songRepository, IUserRepository userRepository, IMapper mapper)
    {
        _songRepository = songRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Song> AddSongAsync(SongPublishedDto songPublishedDto, CancellationToken cancellationToken)
    {
        var song = _mapper.Map<Song>(songPublishedDto);
        var artist = await _userRepository.GetUserByUsernameAsync(song.Artist.Username, cancellationToken);
        song.Artist = artist;

        await _songRepository.CreateAsync(song, cancellationToken);
        await _songRepository.SaveChangesAsync(cancellationToken);

        return song;
    }
}
