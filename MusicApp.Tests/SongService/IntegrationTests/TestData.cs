using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.Tests.SongService.IntegrationTests;

public static class TestData
{
    public static Artist Artist = new()
    {
        Id = new Guid("40543b82-9493-40f7-a1c8-04f41f844bf1"),
        Username = "Anton"
    };

    public static ArtistOutputDto ArtistOutputDto = new()
    {
        Id = new Guid("40543b82-9493-40f7-a1c8-04f41f844bf1"),
        Username = "Anton"
    };

    public static Song Song = new()
    {
        Id = new Guid("2d007faf-4162-4470-90aa-3ce56afe54fa"),
        Title = "Song",
        Artist = Artist
    };

    public static SongOutputDto SongOutputDto = new()
    {
        Id = new Guid("2d007faf-4162-4470-90aa-3ce56afe54fa"),
        Title = "Song",
        Artist = ArtistOutputDto
    };

    public static SongInputDto SongInputDto = new()
    {
        Title = "song"
    };

    public static SongInputDto SongWithIncorrectTitle = new()
    {
        Title = "a"
    };
}