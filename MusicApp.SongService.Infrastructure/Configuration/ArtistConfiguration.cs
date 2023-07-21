using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Infrastructure.Configuration;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.HasKey(artist => artist.Id);
        builder.Property(artist => artist.Username).IsRequired().HasMaxLength(32);
        builder.HasMany(artist => artist.Songs).WithOne(song => song.Artist);
    }
}
