using Microsoft.EntityFrameworkCore;
using MusicApp.Identity.Domain.Entities;

namespace MusicApp.Identity.Infrastructure.Extensions;

public static class ModelBuilderExtension
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = Guid.NewGuid(),
                Title = "artist"
            }
        );
    }
}