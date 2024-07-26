using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using ShortenerEndpoint.Models;

namespace ShortenerEndpoint.Infrastructure.Contexts;

public sealed class ShortenDbContext : DbContext
{
    public ShortenDbContext(DbContextOptions<ShortenDbContext> options) : base(options) { }

    public DbSet<UrlLink> UrlLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UrlLink>().ToCollection(nameof(UrlLinks));
    }
}