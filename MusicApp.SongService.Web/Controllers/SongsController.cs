using Microsoft.AspNetCore.Mvc;
using MusicApp.SongService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using MusicApp.SongService.Application.Services.Interfaces;

namespace MusicApp.SongService.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongsController : ControllerBase
{
    private readonly ISongsService _service;

    public SongsController(ISongsService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllSongs()
    {
        return Ok(await _service.GetAllSongs());
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSongById(Guid id)
    {
        return Ok(await _service.GetSongById(id));
    }
    
    [HttpPost, Authorize(Roles = "artist")]
    public async Task<IActionResult> CreateSong(Song song)
    {
        var username = User.Identity.Name;

        await _service.CreateSong(song, username);

        return StatusCode(201);
    }
    
    [HttpPut("{id:guid}"), Authorize(Roles = "artist")]
    public async Task<IActionResult> UpdateSong(Song song)
    {
        var username = User.Identity.Name;

        await _service.UpdateSong(song, username);

        return Ok();
    }

    [HttpDelete("{id:guid}"), Authorize(Roles = "artist")]
    public async Task<IActionResult> DeleteSong(Guid id)
    {
        var username = User.Identity.Name;

        await _service.DeleteSong(id, username);

        return StatusCode(204);
    }
}