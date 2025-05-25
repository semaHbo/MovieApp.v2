using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieApp.v2.Web.Models;

namespace MovieApp.v2.Web.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed Genres
        builder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Aksiyon", Description = "Aksiyon filmleri" },
            new Genre { Id = 2, Name = "Komedi", Description = "Komedi filmleri" },
            new Genre { Id = 3, Name = "Drama", Description = "Drama filmleri" },
            new Genre { Id = 4, Name = "Bilim Kurgu", Description = "Bilim kurgu filmleri" },
            new Genre { Id = 5, Name = "Korku", Description = "Korku filmleri" }
        );
    }
}
