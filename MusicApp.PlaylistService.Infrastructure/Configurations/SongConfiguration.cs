using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Infrastructure.Configurations;

public class SongConfiguration : IEntityTypeConfiguration<Song>
{
    public void Configure(EntityTypeBuilder<Song> builder)
    {
        builder.HasKey(song => song.Id);
        builder.Property(song => song.Title).IsRequired().HasMaxLength(32);
        builder.HasOne(song => song.Artist).WithMany(artist => artist.Songs);
    }
}
