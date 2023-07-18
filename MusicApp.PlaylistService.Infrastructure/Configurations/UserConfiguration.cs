using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Username).IsRequired().HasMaxLength(32);
        builder.HasMany(a => a.Songs).WithOne(s => s.Artist);
        builder.HasMany(a => a.Playlists).WithOne(p => p.Creator);
    }
}
