using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Infrastructure.Configuration;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Username).IsRequired().HasMaxLength(32);
        builder.HasMany(a => a.Songs).WithOne(s => s.Artist);
    }
}
