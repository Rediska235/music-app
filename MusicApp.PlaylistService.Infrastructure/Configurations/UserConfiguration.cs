using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Username).IsRequired().HasMaxLength(32);
        builder.HasMany(user => user.Songs).WithOne(song => song.Artist);
        builder.HasMany(user => user.Playlists).WithOne(playlist => playlist.Creator);
    }
}
