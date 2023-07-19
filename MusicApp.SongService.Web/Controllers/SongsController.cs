using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.SongService.Application.CQRS.Commands.CreateSong;
using MusicApp.SongService.Application.CQRS.Commands.DeleteSong;
using MusicApp.SongService.Application.CQRS.Commands.EnsureArtistCreated;
using MusicApp.SongService.Application.CQRS.Commands.UpdateSong;
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
        var songs = await _mediator.Send(new GetSongsQuery(), cancellationToken);

        return Ok(songs);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSongById(Guid id, CancellationToken cancellationToken)
    {
        var song = await _mediator.Send(new GetSongByIdQuery(id), cancellationToken);

        return Ok(song);
    }
        
    [HttpPost]
    [Authorize(Roles = "artist")]
    public async Task<IActionResult> CreateSong(SongInputDto songInputDto, CancellationToken cancellationToken)
    {
        var artist = await _mediator.Send(new EnsureArtistCreatedCommand(), cancellationToken);

        var song = await _mediator.Send(new CreateSongCommand(songInputDto, artist), cancellationToken);

        return Created($"/api/songs/{song.Id}", song);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "artist")]
    public async Task<IActionResult> UpdateSong([FromRoute] Guid id, [FromBody] SongInputDto songInputDto, CancellationToken cancellationToken)
    {
        var song = await _mediator.Send(new UpdateSongCommand(id, songInputDto), cancellationToken);

        return Ok(song);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "artist")]
    public async Task<IActionResult> DeleteSong(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteSongCommand(id), cancellationToken);

        return NoContent();
    }
}