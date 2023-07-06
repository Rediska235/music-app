using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Infrastructure.Extensions;

public static class ModelBuilderExtension
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "petr"
        };

        modelBuilder.Entity<User>().HasData(user);

        modelBuilder.Entity<Song>().HasData(
            new Song
            {
                Id = Guid.NewGuid(),
                Title = "1",
                Artist = user
            },
            new Song
            {
                Id = Guid.NewGuid(),
                Title = "2",
                Artist = user
            },
            new Song
            {
                Id = Guid.NewGuid(),
                Title = "3",
                Artist = user
            }
        );
    }
}