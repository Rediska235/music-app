using AutoMapper;
using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.AddArtist;

public class AddArtistCommandHandler : IRequestHandler<AddArtistCommand>
{
    private readonly IArtistRepository _repository;
    private readonly IMapper _mapper;

    public AddArtistCommandHandler(IArtistRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Handle(AddArtistCommand request, CancellationToken cancellationToken)
    {
        var artist = _mapper.Map<Artist>(request.UserPublishedDto);

        await _repository.CreateAsync(artist, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}