using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicApp.SongService.Application.CQRS.Commands.CreateSong;
using MusicApp.SongService.Application.CQRS.Commands.CreateSongDelayed;
using MusicApp.SongService.Application.CQRS.Commands.DeleteSong;
using MusicApp.SongService.Application.CQRS.Commands.LikeSong;
using MusicApp.SongService.Application.CQRS.Commands.UpdateSong;
using MusicApp.SongService.Application.CQRS.Queries.GetArtist;
using MusicApp.SongService.Application.CQRS.Queries.GetFavoriteSongs;
using MusicApp.SongService.Application.CQRS.Queries.GetSongById;
using MusicApp.SongService.Application.CQRS.Queries.GetSongs;
using MusicApp.SongService.Application.DTOs;

namespace MusicApp.SongService.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SongsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetSongs(CancellationToken cancellationToken)
    {
        var getQuery = new GetSongsQuery();
        var songs = await _mediator.Send(getQuery, cancellationToken);

        return Ok(songs);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSongById(Guid id, CancellationToken cancellationToken)
    {
        var getByIdQuery = new GetSongByIdQuery(id);
        var song = await _mediator.Send(getByIdQuery, cancellationToken);

        return Ok(song);
    }
        
    [HttpPost]
    public async Task<IActionResult> CreateSong(SongInputDto songInputDto, CancellationToken cancellationToken)
    {
        var getArtistQuery = new GetArtistQuery();
        var artist = await _mediator.Send(getArtistQuery, cancellationToken);

        var createCommand = new CreateSongCommand(songInputDto, artist);
        var song = await _mediator.Send(createCommand, cancellationToken);

        var actionName = nameof(GetSongById);
        var routeValues = new { id = song.Id };

        return CreatedAtAction(actionName, routeValues, song);
    }

    [HttpPost("delayed")]
    public async Task<IActionResult> CreateSongDelayed(DelayedSongInputDto delayedSongInputDto, CancellationToken cancellationToken)
    {
        var getArtistQuery = new GetArtistQuery();
        var artist = await _mediator.Send(getArtistQuery, cancellationToken);

        var createCommand = new CreateSongDelayedCommand(delayedSongInputDto, artist);
        await _mediator.Send(createCommand, cancellationToken);

        return Ok($"{delayedSongInputDto.Title} will be published at {delayedSongInputDto.PublishTime.ToString("HH:mm:ss")}");
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateSong([FromRoute] Guid id, [FromBody] SongInputDto songInputDto, CancellationToken cancellationToken)
    {
        var updateCommand = new UpdateSongCommand(id, songInputDto);
        var song = await _mediator.Send(updateCommand, cancellationToken);

        return Ok(song);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSong(Guid id, CancellationToken cancellationToken)
    {
        var deleteCommand = new DeleteSongCommand(id);
        await _mediator.Send(deleteCommand, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> LikeSong(Guid id, CancellationToken cancellationToken)
    {
        var likeCommand = new LikeSongCommand(id);
        await _mediator.Send(likeCommand, cancellationToken);

        return Ok();
    }

    [HttpGet("favorite")]
    public async Task<IActionResult> GetFavoriteSongs(CancellationToken cancellationToken)
    {
        var getQuery = new GetFavoriteSongsQuery();
        var songs = await _mediator.Send(getQuery, cancellationToken);

        return Ok(songs);
    }
}