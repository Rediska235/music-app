using AutoMapper;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.PlaylistService.Domain.Entities;

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

    public async Task<IEnumerable<SongOutputDto>> UpdateSongs(IEnumerable<Song> songs, CancellationToken cancellationToken)
    {
        await _songRepository.DeleteAllSongs(cancellationToken);

        foreach(var song in songs)
        {
            var artist = await _userRepository.GetUserByUsernameAsync(song.Artist.Username, cancellationToken);
            if(artist != null)
            {
                song.Artist = artist;
            }

            await _songRepository.CreateAsync(song, cancellationToken);
            await _songRepository.SaveChangesAsync(cancellationToken);
        }

        return _mapper.Map<IEnumerable<SongOutputDto>>(songs);
    }
}
