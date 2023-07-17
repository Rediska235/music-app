using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Infrastructure.Configuration;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Username).IsRequired().HasMaxLength(32);
    }
}