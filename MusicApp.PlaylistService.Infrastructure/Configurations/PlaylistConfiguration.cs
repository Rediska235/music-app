using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Infrastructure.Configurations;

public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(32);
        builder.HasOne(p => p.Creator).WithMany(c => c.Playlists);
        builder.HasMany(p => p.Songs)
            .WithMany(s => s.Playlists)
            .UsingEntity<PlaylistSong>(
                j => j.HasOne<Song>().WithMany().OnDelete(DeleteBehavior.NoAction),
                j => j.HasOne<Playlist>().WithMany().OnDelete(DeleteBehavior.Cascade));
    }
}
