using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.SongService.Application.CQRS.Commands.CreateSong;
using MusicApp.SongService.Application.CQRS.Commands.DeleteSong;
using MusicApp.SongService.Application.CQRS.Commands.EnsureArtistCreated;
using MusicApp.SongService.Application.CQRS.Commands.UpdateSong;
using MusicApp.SongService.Application.CQRS.Queries.GetSongs;
using MusicApp.SongService.Application.CQRS.Queries.GetSongById;
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
        return Ok(await _mediator.Send(new GetSongsQuery(), cancellationToken));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSongById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetSongByIdQuery(id), cancellationToken));
    }
    
    [HttpPost, Authorize(Roles = "artist")]
    public async Task<IActionResult> CreateSong(SongInputDto song, CancellationToken cancellationToken)
    {
        var artist = await _mediator.Send(new EnsureArtistCreatedCommand());

        await _mediator.Send(new CreateSongCommand(song, artist), cancellationToken);

        return StatusCode(201);
    }
    
    [HttpPut("{id:guid}"), Authorize(Roles = "artist")]
    public async Task<IActionResult> UpdateSong([FromRoute] Guid id, [FromBody] SongInputDto song, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateSongCommand(id, song), cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:guid}"), Authorize(Roles = "artist")]
    public async Task<IActionResult> DeleteSong(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteSongCommand(id), cancellationToken);

        return StatusCode(204);
    }
}