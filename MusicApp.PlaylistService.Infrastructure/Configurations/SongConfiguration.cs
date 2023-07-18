using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Infrastructure.Configurations;

public class SongConfiguration : IEntityTypeConfiguration<Song>
{
    public void Configure(EntityTypeBuilder<Song> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Title).IsRequired().HasMaxLength(32);
        builder.HasOne(s => s.Artist).WithMany(a => a.Songs);
        builder.HasMany(s => s.Playlists)
            .WithMany(p => p.Songs)
            .UsingEntity<PlaylistSong>(
                j => j.HasOne<Playlist>().WithMany().OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Song>().WithMany().OnDelete(DeleteBehavior.NoAction));
    }
}
