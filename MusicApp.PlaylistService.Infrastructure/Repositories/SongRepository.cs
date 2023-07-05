﻿using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Infrastructure.Data;

namespace MusicApp.PlaylistService.Infrastructure.Repositories;

public class SongRepository : ISongRepository
{
    private readonly AppDbContext _db;

    public SongRepository(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<Song> GetSongByIdAsync(Guid id)
    {
        return await _db.Songs.FirstOrDefaultAsync(s => s.Id == id);
    }
}
