using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.Tests.PlaylistService.IntegrationTests;

public static class TestData
{
    public static User User = new()
    {
        Id = new Guid("40543b82-9493-40f7-a1c8-04f41f844bf1"),
        Username = "Anton"
    };

    public static UserOutputDto UserOutputDto = new()
    {
        Id = new Guid("40543b82-9493-40f7-a1c8-04f41f844bf1"),
        Username = "Anton"
    };

    public static Playlist Playlist = new()
    {
        Id = new Guid("2d007faf-4162-4470-90aa-3ce59afe52fa"),
        Name = "TestPlaylist",
        IsPrivate = false,
        Creator = User
    };

    public static PlaylistOutputDto PlaylistOutputDto = new()
    {
        Id = new Guid("2d007faf-4162-4470-90aa-3ce59afe52fa"),
        Name = "TestPlaylist",
        IsPrivate = false,
        Creator = UserOutputDto
    };

    public static PlaylistInputDto PlaylistInputDto = new()
    {
        Name = "TestPlaylist",
        IsPrivate = false
    };

    public static PlaylistInputDto UpdatedPlaylist = new()
    {
        Name = "UpdatedTestPlaylist",
        IsPrivate = true
    };

    public static Song Song = new()
    {
        Id = new Guid("2d007faf-4162-4470-90aa-3ce56afe54fa"),
        Title = "Song",
        Artist = User
    };

    public static SongOutputDto SongOutputDto = new()
    {
        Id = new Guid("2d007faf-4162-4470-90aa-3ce56afe54fa"),
        Title = "Song",
        Artist = UserOutputDto
    };
}