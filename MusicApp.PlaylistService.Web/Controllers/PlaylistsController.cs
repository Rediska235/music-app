using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.PlaylistService.Web.Filters;

namespace MusicApp.PlaylistService.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistsController : ControllerBase
{
    private readonly IPlaylistsService _service;

    public PlaylistsController(IPlaylistsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlaylists(CancellationToken cancellationToken)
    {
        return Ok(await _service.GetPlaylistsAsync(cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPlaylistById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await _service.GetPlaylistByIdAsync(id, cancellationToken));
    }

    [HttpPost, Authorize]
    [ValidationFilter]
    public async Task<IActionResult> CreatePlaylist(PlaylistInputDto playlist, CancellationToken cancellationToken)
    {
        await _service.CreatePlaylistAsync(playlist, cancellationToken);

        return StatusCode(201);
    }

    [HttpPut("{id:guid}"), Authorize]
    [ValidationFilter]
    public async Task<IActionResult> UpdatePlaylist([FromRoute] Guid id, [FromBody] PlaylistInputDto playlist, CancellationToken cancellationToken)
    {
        await _service.UpdatePlaylistAsync(id, playlist, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:guid}"), Authorize]
    public async Task<IActionResult> DeletePlaylist(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeletePlaylistAsync(id, cancellationToken);

        return StatusCode(204);
    }

    [HttpPatch("{playlistId:guid}/add/{songId:guid}"), Authorize]
    public async Task<IActionResult> AddSong(Guid playlistId, Guid songId, CancellationToken cancellationToken)
    {
        await _service.AddSongAsync(playlistId, songId, cancellationToken);

        return Ok();
    }

    [HttpPatch("{playlistId:guid}/remove/{songId:guid}"), Authorize]
    public async Task<IActionResult> RemoveSong(Guid playlistId, Guid songId, CancellationToken cancellationToken)
    {
        await _service.RemoveSongAsync(playlistId, songId, cancellationToken);

        return Ok();
    }
}
