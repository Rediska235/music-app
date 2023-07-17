using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Infrastructure.Configuration;

public class SongConfiguration : IEntityTypeConfiguration<Song>
{
    public void Configure(EntityTypeBuilder<Song> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Title).IsRequired().HasMaxLength(32);
        builder.HasOne(s => s.Artist).WithMany(a => a.Songs);
    }
}
