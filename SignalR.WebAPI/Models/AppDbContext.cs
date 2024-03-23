﻿using Microsoft.EntityFrameworkCore;

namespace SignalR.WebAPI.Models;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Team> Teams { get; set; }
}
