using MediatR;
using Microsoft.AspNetCore.Http;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.EnsureArtistCreated;

public class EnsureArtistCreatedCommandHandler : IRequestHandler<EnsureArtistCreatedCommand, Artist>
{
    private readonly IArtistRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EnsureArtistCreatedCommandHandler(IArtistRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Artist> Handle(EnsureArtistCreatedCommand request, CancellationToken cancellationToken)
    {
        var username = _httpContextAccessor.HttpContext.User.Identity.Name;
        var artist = await _repository.GetArtistByUsernameAsync(username);
        if(artist != null)
        {
            return artist;
        }

        artist = new Artist()
        {
            Username = username
        };

        await _repository.CreateArtistAsync(artist);
        await _repository.SaveChangesAsync();

        return artist;
    }
}