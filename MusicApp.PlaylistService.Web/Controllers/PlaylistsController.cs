using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.PlaylistService.Web.Filters;
using MusicApp.PlaylistService.Web.Grpc;

namespace MusicApp.PlaylistService.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistsController : ControllerBase
{
    private readonly IPlaylistsService _playlistService;
    private readonly ISongService _songService;
    private readonly GrpcSongClient _grpcClient;

    public PlaylistsController(IPlaylistsService playlistService, ISongService songService, GrpcSongClient grpcClient)
    {
        _playlistService = playlistService;
        _songService = songService;
        _grpcClient = grpcClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlaylists(CancellationToken cancellationToken)
    {
        var playlists = await _playlistService.GetPlaylistsAsync(cancellationToken);

        return Ok(playlists);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPlaylistById(Guid id, CancellationToken cancellationToken)
    {
        var playlists = await _playlistService.GetPlaylistByIdAsync(id, cancellationToken);

        return Ok(playlists);
    }

    [HttpPost]
    [Authorize]
    [ValidationFilter]
    public async Task<IActionResult> CreatePlaylist(PlaylistInputDto playlistInputDto, CancellationToken cancellationToken)
    {
        var playlist = await _playlistService.CreatePlaylistAsync(playlistInputDto, cancellationToken);

        var actionName = nameof(GetPlaylistById); 
        var routeValues = new { id = playlist.Id};

        return CreatedAtAction(actionName, routeValues, playlist);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [ValidationFilter]
    public async Task<IActionResult> UpdatePlaylist([FromRoute] Guid id, [FromBody] PlaylistInputDto playlistInputDto, CancellationToken cancellationToken)
    {
        var playlist = await _playlistService.UpdatePlaylistAsync(id, playlistInputDto, cancellationToken);

        return Ok(playlist);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeletePlaylist(Guid id, CancellationToken cancellationToken)
    {
        await _playlistService.DeletePlaylistAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{playlistId:guid}/add/{songId:guid}")]
    [Authorize]
    public async Task<IActionResult> AddSong(Guid playlistId, Guid songId, CancellationToken cancellationToken)
    {
        await _playlistService.AddSongAsync(playlistId, songId, cancellationToken);

        return Ok();
    }

    [HttpPatch("{playlistId:guid}/remove/{songId:guid}")]
    [Authorize]
    public async Task<IActionResult> RemoveSong(Guid playlistId, Guid songId, CancellationToken cancellationToken)
    {
        await _playlistService.RemoveSongAsync(playlistId, songId, cancellationToken);
        
        return Ok();
    }
    
    [HttpGet("updateSongs")]
    public async Task<IActionResult> UpdateSongs(CancellationToken cancellationToken)
    {
        var songs = _grpcClient.GetSongsFromSongService();
        
        var result = await _songService.UpdateSongs(songs, cancellationToken);

        return Ok(result);
    }
}
