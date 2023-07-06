using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.PlaylistService.Domain.Entities;

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
    public async Task<IActionResult> GetAllPlaylists()
    {
        var username = User.Identity.Name;

        return Ok(await _service.GetAllPlaylists(username));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPlaylistById(Guid id)
    {
        var username = User.Identity.Name;

        return Ok(await _service.GetPlaylistById(id, username));
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> CreatePlaylist(Playlist playlist)
    {
        var username = User.Identity.Name;

        await _service.CreatePlaylist(playlist, username);

        return StatusCode(201);
    }

    [HttpPut("{id:guid}"), Authorize]
    public async Task<IActionResult> UpdatePlaylist(Playlist playlist)
    {
        var username = User.Identity.Name;

        await _service.UpdatePlaylist(playlist, username);

        return Ok();
    }

    [HttpDelete("{id:guid}"), Authorize]
    public async Task<IActionResult> DeletePlaylist(Guid id)
    {
        var username = User.Identity.Name;

        await _service.DeletePlaylist(id, username);

        return StatusCode(204);
    }

    [HttpPatch("{playlistId:guid}/add/{songId:guid}"), Authorize]
    public async Task<IActionResult> AddSong(Guid playlistId, Guid songId)
    {
        var username = User.Identity.Name;

        await _service.AddSong(playlistId, songId, username);

        return Ok();
    }

    [HttpPatch("{playlistId:guid}/remove/{songId:guid}"), Authorize]
    public async Task<IActionResult> RemoveSong(Guid playlistId, Guid songId)
    {
        var username = User.Identity.Name;

        await _service.RemoveSong(playlistId, songId, username);

        return Ok();
    }
}
