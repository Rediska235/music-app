using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Application.Services.Implementations;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Domain.Exceptions;

namespace MusicApp.Tests.PlaylistService.UnitTests.Services;

public class PlaylistServiceTests
{
    private readonly Mock<IPlaylistRepository> _playlistRepositoryMock = new();
    private readonly Mock<ISongRepository> _songRepositoryMock = new();
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<PlaylistsService>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly CancellationToken _cancellationToken = new();
    private readonly PlaylistsService _service;
    
    public PlaylistServiceTests()
    {
        _service = new PlaylistsService(
            _playlistRepositoryMock.Object,
            _songRepositoryMock.Object,
            _userServiceMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async void GetPlaylistsAsync_ShouldReturnPlaylistOutputDtos()
    {
        //Arrange
        var publicPlaylists = _fixture.CreateMany<Playlist>();
        var myPrivatePlaylists = _fixture.CreateMany<Playlist>();
        var allPlaylists = publicPlaylists.Concat(myPrivatePlaylists);

        var outputPlaylists = _fixture.CreateMany<PlaylistOutputDto>();

        _playlistRepositoryMock.Setup(_playlistRepositoryMock =>
            _playlistRepositoryMock.GetPublicPlaylistsAsync(_cancellationToken))
                .ReturnsAsync(publicPlaylists);

        _playlistRepositoryMock.Setup(_playlistRepositoryMock =>
            _playlistRepositoryMock.GetMyPrivatePlaylistsAsync(It.IsAny<string>(), _cancellationToken))
            .ReturnsAsync(myPrivatePlaylists);

        _mapperMock.Setup(_mapperMock =>
            _mapperMock.Map<IEnumerable<PlaylistOutputDto>>(allPlaylists))
                .Returns(outputPlaylists);

        //Act
        var result = await _service.GetPlaylistsAsync(_cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(outputPlaylists);
    }

    [Fact]
    public async void GetPlaylistByIdAsync_WhenPlaylistNotFound_ShouldThrowException()
    {
        //Arrange
        var id = _fixture.Create<Guid>();

        //Act
        var act = async() => await _service.GetPlaylistByIdAsync(id, _cancellationToken);

        //Assert
        await act.Should().ThrowAsync<PlaylistNotFoundException>();
    }

    [Fact]
    public async void GetPlaylistByIdAsync_WhenNotYourPrivatePlaylist_ShouldThrowException()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var username = "your name";
        var playlist = new Playlist
        {
            Id = id,
            IsPrivate = true,
            Creator = new User
            {
                Username = "not your name"
            }
        };

        _playlistRepositoryMock.Setup(_playlistRepositoryMock =>
            _playlistRepositoryMock.GetByIdAsync(id, _cancellationToken))
                .ReturnsAsync(playlist);

        _userServiceMock.Setup(_userServiceMock => _userServiceMock.GetUsername()).Returns(username);

        //Act
        var act = async () => await _service.GetPlaylistByIdAsync(id, _cancellationToken);

        //Assert
        await act.Should().ThrowAsync<PrivatePlaylistException>();
    }

    [Fact]
    public async void GetPlaylistByIdAsync_ShouldReturnPlaylistOutputDto()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var playlist = _fixture.Create<Playlist>();
        playlist.IsPrivate = false;

        var playlistOutputDto = _fixture.Create<PlaylistOutputDto>();

        _playlistRepositoryMock.Setup(_playlistRepositoryMock =>
            _playlistRepositoryMock.GetByIdAsync(id, _cancellationToken))
                .ReturnsAsync(playlist);

        _mapperMock.Setup(_mapperMock =>
            _mapperMock.Map<PlaylistOutputDto>(playlist))
                .Returns(playlistOutputDto);

        //Act
        var result = await _service.GetPlaylistByIdAsync(id, _cancellationToken);

        //Assert
        result.Should().Be(playlistOutputDto);
    }

    [Fact]
    public async void CreatePlaylistAsync_ShouldReturnCreatedPlaylistOutputDtoWithCorrectAuthor()
    {
        //Arrange
        var user = _fixture.Create<User>();
        var playlistInputDto = _fixture.Create<PlaylistInputDto>();
        var playlist = _fixture.Create<Playlist>();
        var playlistOutputDto = _fixture.Create<PlaylistOutputDto>();
        playlistOutputDto.Creator.Id = user.Id;

        _mapperMock.Setup(_mapperMock =>
                   _mapperMock.Map<Playlist>(playlistInputDto))
                .Returns(playlist);

        _userServiceMock.Setup(_userServiceMock => _userServiceMock.GetUserAsync(_cancellationToken)).ReturnsAsync(user);

        _mapperMock.Setup(_mapperMock =>
                   _mapperMock.Map<PlaylistOutputDto>(playlist))
                .Returns(playlistOutputDto);

        //Act
        var result = await _service.CreatePlaylistAsync(playlistInputDto, _cancellationToken);

        //Assert
        result.Should().Be(playlistOutputDto);
        result.Creator.Id.Should().Be(user.Id);
    }

    [Fact]
    public async void UpdatePlaylistAsync_WhenPlaylistNotFound_ShouldReturnPlaylistOutputDto()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var playlistInputDto = _fixture.Create<PlaylistInputDto>();

        //Act
        var act = async () => await _service.UpdatePlaylistAsync(id, playlistInputDto, _cancellationToken);

        //Assert
        await act.Should().ThrowAsync<PlaylistNotFoundException>();
    }

    [Fact]
    public async void UpdatePlaylistAsync_WhenNotYourPlaylist_ShouldReturnPlaylistOutputDto()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var playlistInputDto = _fixture.Create<PlaylistInputDto>();
        var playlist = _fixture.Create<Playlist>();

        _playlistRepositoryMock.Setup(_playlistRepositoryMock =>
            _playlistRepositoryMock.GetByIdAsync(id, _cancellationToken))
                .ReturnsAsync(playlist);

        _userServiceMock.Setup(_userServiceMock => _userServiceMock.ValidateOwnerAndThrow(playlist)).Throws<NotYourPlaylistException>();

        //Act
        var act = async () => await _service.UpdatePlaylistAsync(id, playlistInputDto, _cancellationToken);

        //Assert
        await act.Should().ThrowAsync<NotYourPlaylistException>();
    }

    [Fact]
    public async void UpdatePlaylistAsync_ShouldReturnUpdatedPlaylistOutputDto()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var playlistInputDto = _fixture.Create<PlaylistInputDto>();
        var playlist = _fixture.Create<Playlist>();
        var playlistOutputDto = _fixture.Create<PlaylistOutputDto>();

        _playlistRepositoryMock.Setup(_playlistRepositoryMock =>
            _playlistRepositoryMock.GetByIdAsync(id, _cancellationToken))
                .ReturnsAsync(playlist);

        _mapperMock.Setup(_mapperMock =>
                   _mapperMock.Map<Playlist>(playlistInputDto))
                .Returns(playlist);

        _mapperMock.Setup(_mapperMock =>
                   _mapperMock.Map<PlaylistOutputDto>(playlist))
                .Returns(playlistOutputDto);

        //Act
        var result = await _service.UpdatePlaylistAsync(id, playlistInputDto, _cancellationToken);

        //Assert
        result.Should().Be(playlistOutputDto);
    }

    [Fact]
    public async void DeletePlaylistAsync_WhenPlaylistNotFound_ShouldReturnPlaylistOutputDto()
    {
        //Arrange
        var id = _fixture.Create<Guid>();

        //Act
        var act = async () => await _service.DeletePlaylistAsync(id, _cancellationToken);

        //Assert
        await act.Should().ThrowAsync<PlaylistNotFoundException>();
    }

    [Fact]
    public async void DeletePlaylistAsync_WhenNotYourPlaylist_ShouldReturnPlaylistOutputDto()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var playlist = _fixture.Create<Playlist>();

        _playlistRepositoryMock.Setup(_playlistRepositoryMock =>
            _playlistRepositoryMock.GetByIdAsync(id, _cancellationToken))
                .ReturnsAsync(playlist);

        _userServiceMock.Setup(_userServiceMock => _userServiceMock.ValidateOwnerAndThrow(playlist)).Throws<NotYourPlaylistException>();

        //Act
        var act = async () => await _service.DeletePlaylistAsync(id, _cancellationToken);

        //Assert
        await act.Should().ThrowAsync<NotYourPlaylistException>();
    }

    [Fact]
    public async void DeletePlaylistAsync_ShouldDeletePlaylist()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var playlist = _fixture.Create<Playlist>();

        _playlistRepositoryMock.Setup(_playlistRepositoryMock =>
            _playlistRepositoryMock.GetByIdAsync(id, _cancellationToken))
                .ReturnsAsync(playlist);

        //Act
        var act = async () => await _service.DeletePlaylistAsync(id, _cancellationToken);

        //Assert
        await act.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async void AddSongAsync_WhenPlaylistNotFound_ShouldReturnPlaylistOutputDto()
    {
        //Arrange
        var playlistId = _fixture.Create<Guid>();
        var songId = _fixture.Create<Guid>();

        //Act
        var act = async () => await _service.AddSongAsync(playlistId, songId, _cancellationToken);

        //Assert
        await act.Should().ThrowAsync<PlaylistNotFoundException>();
    }

    [Fact]
    public async void AddSongAsync_WhenNotYourPlaylist_ShouldReturnPlaylistOutputDto()
    {
        //Arrange
        var playlistId = _fixture.Create<Guid>();
        var songId = _fixture.Create<Guid>();
        var playlist = _fixture.Create<Playlist>();

        _playlistRepositoryMock.Setup(_playlistRepositoryMock =>
            _playlistRepositoryMock.GetByIdAsync(playlistId, _cancellationToken))
                .ReturnsAsync(playlist);

        _userServiceMock.Setup(_userServiceMock => _userServiceMock.ValidateOwnerAndThrow(playlist)).Throws<NotYourPlaylistException>();

        //Act
        var act = async () => await _service.AddSongAsync(playlistId, songId, _cancellationToken);

        //Assert
        await act.Should().ThrowAsync<NotYourPlaylistException>();
    }

    [Fact]
    public async void AddSongAsync_WhenSongNotFound_ShouldReturnPlaylistOutputDto()
    {
        //Arrange
        var playlistId = _fixture.Create<Guid>();
        var songId = _fixture.Create<Guid>();
        var playlist = _fixture.Create<Playlist>();

        _playlistRepositoryMock.Setup(_playlistRepositoryMock =>
            _playlistRepositoryMock.GetByIdAsync(playlistId, _cancellationToken))
                .ReturnsAsync(playlist);

        //Act
        var act = async () => await _service.AddSongAsync(playlistId, songId, _cancellationToken);

        //Assert
        await act.Should().ThrowAsync<SongNotFoundException>();
    }

    [Fact]
    public async void AddSongAsync_ShouldAddSongToPlaylist()
    {
        //Arrange
        var playlistId = _fixture.Create<Guid>();
        var songId = _fixture.Create<Guid>();
        var playlist = _fixture.Create<Playlist>();
        var song = _fixture.Create<Song>();

        _playlistRepositoryMock.Setup(_playlistRepositoryMock =>
            _playlistRepositoryMock.GetByIdAsync(playlistId, _cancellationToken))
                .ReturnsAsync(playlist);

        _songRepositoryMock.Setup(_songRepositoryMock =>
            _songRepositoryMock.GetByIdAsync(songId, _cancellationToken))
                .ReturnsAsync(song);

        //Act
        var act = async () => await _service.AddSongAsync(playlistId, songId, _cancellationToken);

        //Assert
        await act.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async void RemoveSongAsync_WhenPlaylistNotFound_ShouldReturnPlaylistOutputDto()
    {
        //Arrange
        var playlistId = _fixture.Create<Guid>();
        var songId = _fixture.Create<Guid>();

        //Act
        var act = async () => await _service.RemoveSongAsync(playlistId, songId, _cancellationToken);

        //Assert
        await act.Should().ThrowAsync<PlaylistNotFoundException>();
    }

    [Fact]
    public async void RemoveSongAsync_WhenNotYourPlaylist_ShouldReturnPlaylistOutputDto()
    {
        //Arrange
        var playlistId = _fixture.Create<Guid>();
        var songId = _fixture.Create<Guid>();
        var playlist = _fixture.Create<Playlist>();

        _playlistRepositoryMock.Setup(_playlistRepositoryMock =>
            _playlistRepositoryMock.GetByIdAsync(playlistId, _cancellationToken))
                .ReturnsAsync(playlist);

        _userServiceMock.Setup(_userServiceMock => _userServiceMock.ValidateOwnerAndThrow(playlist)).Throws<NotYourPlaylistException>();

        //Act
        var act = async () => await _service.RemoveSongAsync(playlistId, songId, _cancellationToken);

        //Assert
        await act.Should().ThrowAsync<NotYourPlaylistException>();
    }

    [Fact]
    public async void RemoveSongAsync_WhenSongNotFound_ShouldReturnPlaylistOutputDto()
    {
        //Arrange
        var playlistId = _fixture.Create<Guid>();
        var songId = _fixture.Create<Guid>();
        var playlist = _fixture.Create<Playlist>();

        _playlistRepositoryMock.Setup(_playlistRepositoryMock =>
            _playlistRepositoryMock.GetByIdAsync(playlistId, _cancellationToken))
                .ReturnsAsync(playlist);

        //Act
        var act = async () => await _service.RemoveSongAsync(playlistId, songId, _cancellationToken);

        //Assert
        await act.Should().ThrowAsync<SongNotFoundException>();
    }

    [Fact]
    public async void RemoveSongAsync_ShouldRemoveSongFromPlaylist()
    {
        //Arrange
        var playlistId = _fixture.Create<Guid>();
        var songId = _fixture.Create<Guid>();
        var playlist = _fixture.Create<Playlist>();
        var song = _fixture.Create<Song>();

        _playlistRepositoryMock.Setup(_playlistRepositoryMock =>
            _playlistRepositoryMock.GetByIdAsync(playlistId, _cancellationToken))
                .ReturnsAsync(playlist);

        _songRepositoryMock.Setup(_songRepositoryMock =>
            _songRepositoryMock.GetByIdAsync(songId, _cancellationToken))
                .ReturnsAsync(song);

        //Act
        var act = async () => await _service.RemoveSongAsync(playlistId, songId, _cancellationToken);

        //Assert
        await act.Should().NotThrowAsync<Exception>();
    }
}