using MediatR;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Domain.Exceptions;
using MusicApp.SongService.Application.Services.Interfaces;
using MusicApp.SongService.Application.CQRS.Queries.GetAllSongs;
using MusicApp.SongService.Application.CQRS.Queries.GetSongById;
using MusicApp.SongService.Application.CQRS.Commands.CreateSong;
using MusicApp.SongService.Application.CQRS.Commands.UpdateSong;
using MusicApp.SongService.Application.CQRS.Commands.DeleteSong;
using MusicApp.SongService.Application.CQRS.Commands.CreateArtist;
using MusicApp.SongService.Application.CQRS.Queries.GetArtistByUsername;

namespace MusicApp.SongService.Application.Services.Implementations;

public class SongsService : ISongsService
{
    private readonly IMediator _mediator;

    public SongsService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IEnumerable<Song>> GetAllSongs()
    {
        return await _mediator.Send(new GetAllSongsQuery());
    }

    public async Task<Song> GetSongById(Guid id)
    {
        var song = await _mediator.Send(new GetSongByIdQuery(id));
        if (song == null)
        {
            throw CommonExceptions.songNotFound;
        }

        return song;
    }

    public async Task CreateSong(Song song, string username)
    {
        var artist = await EnsureUserCreated(username);

        await _mediator.Send(new CreateSongCommand(song, artist));
    }

    public async Task UpdateSong(Song song, string username)
    {
        var title = song.Title;

        song = await _mediator.Send(new GetSongByIdQuery(song.Id));
        if(song == null)
        {
            throw CommonExceptions.songNotFound;
        }    

        if(song.Artist.Username != username)
        {
            throw CommonExceptions.notYourSong;
        }

        song.Title = title;

        await _mediator.Send(new UpdateSongCommand(song));
    }

    public async Task DeleteSong(Guid id, string username)
    {
        var song = await _mediator.Send(new GetSongByIdQuery(id));
        if (song == null)
        {
            throw CommonExceptions.songNotFound;
        }

        if (song.Artist.Username != username)
        {
            throw CommonExceptions.notYourSong;
        }

        await _mediator.Send(new DeleteSongCommand(song));
    }

    private async Task<Artist> EnsureUserCreated(string username)
    {
        var artist = await _mediator.Send(new GetArtistByUsernameQuery(username));
        if (artist == null)
        {
            return await _mediator.Send(new CreateArtistCommand(username));
        }

        return artist;
    }
}
