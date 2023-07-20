using AutoMapper;
using MediatR;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.SongService.Application.CQRS.Commands.UpdateSong;

public class UpdateSongCommandHandler : IRequestHandler<UpdateSongCommand, SongOutputDto>
{
    private readonly ISongRepository _repository;
    private readonly ArtistService _artistService;
    private readonly IMapper _mapper;

    public UpdateSongCommandHandler(ISongRepository repository, ArtistService artistService, IMapper mapper)
    {
        _repository = repository;
        _artistService = artistService;
        _mapper = mapper;
    }

    public async Task<SongOutputDto> Handle(UpdateSongCommand request, CancellationToken cancellationToken)
    {
        var song = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (song == null)
        {
            throw new SongNotFoundException();
        }

        _artistService.ValidateArtistAndThrow(song);

        song.Title = request.Song.Title;

        _repository.Update(song);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SongOutputDto>(song);
    }
}