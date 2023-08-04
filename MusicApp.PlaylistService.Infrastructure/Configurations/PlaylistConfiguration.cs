using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Infrastructure.Configurations;

public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        builder.HasKey(playlist => playlist.Id);
        builder.Property(playlist => playlist.Name).IsRequired().HasMaxLength(32);
        builder.HasOne(playlist => playlist.Creator).WithMany();
        builder.HasMany(playlist => playlist.Songs).WithMany(song => song.Playlists);
    }
}
